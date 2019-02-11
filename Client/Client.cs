//-------------------------------------------------------------------------------------------------
// <copyright file="Client.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     Delta Query sample client.
// 
//     This source is subject to the Sample Client End User License Agreement
//     included in this project.
// </summary>
//
// <remarks />
//
// <disclaimer>
//     THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//     EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
//     WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </disclaimer>
//-------------------------------------------------------------------------------------------------

namespace DeltaQueryClient
{
    using Microsoft.Identity.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Graph;
    using System.Threading.Tasks;

    /// <summary>
    /// Sample implementation of obtaining changes from graph using Delta Query.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Logger to be used for logging output/debug.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="scopes">Scopes needed by the app to run</param>
        /// <param name="clientId">Client ID(Application id) of the app.</param>
        /// <param name="logger">Logger to be used for logging output/debug.</param>
        public Client(
            IEnumerable<string> scopes,
            string clientId,
            string authority,
            ILogger logger)
        {
            this.scopes = scopes;
            this.authority = authority;
            this.clientId = clientId;
            this.logger = logger;
            this.graphServiceClient = new GraphServiceClient(GetAuthorizationProvider());
        }

        /// <summary>
        /// Gets or sets the scopes needed by the app.
        /// </summary>
        private IEnumerable<string> scopes { get; set; }

        /// <summary>
        /// Gets or sets the service principal ID for your application.
        /// </summary>
        private string clientId { get; set; }

        /// <summary>
        /// Gets or sets the authority url for auth
        /// </summary>
        private string authority { get; set; }

        /// <summary>
        /// GraphServiceClient client used for the inner working of the graph calls
        /// </summary>
        private GraphServiceClient graphServiceClient { get; }

        /// <summary>
        /// Calls the Delta Query service and returns the result.
        /// </summary>
        /// <param name="stateToken">
        /// Skip token returned by a previous call to the service or <see langref="null"/>.
        /// </param>
        /// <returns>Result from the Delta Query service.</returns>
        public Task<DeltaQueryResult> DeltaQueryAsync(string stateToken)
        {       
            return this.DeltaQueryAsync(
                stateToken,
                new string[0]);
        }

        /// <summary>
        /// Calls the Delta Query service and returns the result.
        /// </summary>
        /// <param name="entitySet">Name of the entity set to query.</param>
        /// <param name="stateToken">
        /// Skip token returned by a previous call to the service or <see langref="null"/>.
        /// </param>
        /// <param name="propertyList">List of properties to retrieve.</param>
        /// <returns>Result from the Delta Query service.</returns>
        public async Task<DeltaQueryResult> DeltaQueryAsync(
            string stateToken,
            ICollection<string> propertyList)
        {
            //Append any parameters to the query
            List<QueryOption> options = new List<QueryOption>();
            if (propertyList.Any())
            {
                foreach (string parameter in propertyList)
                {
                   options.Add( new QueryOption("$select", parameter));
                }
            }

            //run graph query
            IMailFolderDeltaCollectionPage graphResult;
            try
            {
                graphResult = await graphServiceClient.Me.MailFolders.Delta().Request(options).GetAsync();
            }
            catch (MsalServiceException mse)
            {
                switch (mse.ErrorCode)
                {
                    case MsalServiceException.InvalidAuthority:
                        // What happens:   When the library attempts to discover the authority and get the endpoints it
                        // needs to acquire a token, it got an un-authorize HTTP code or an unexpected response
                        // Remediation:
                        // Check that the authority configured for the application, or passed on some overrides
                        // of token acquisition tokens supporting authority override is correct
                    case "unauthorized_client":
                        // For instance: AADSTS700016: Application with identifier '{clientId}' was not found in the directory '{domain}'.
                        // This can happen if the application has not been installed by the administrator of the tenant or consented to by any user in the tenant. 
                        // You may have sent your authentication request to the wrong tenant
                        // Cause: The clientId in the app.config is wrong
                        // Remediation: check the clientId and the app registration
                        logger.Log("The application is not configured correctly with Azure AD");
                        break;
                    case MsalServiceException.RequestTimeout:
                    case MsalServiceException.ServiceNotAvailable:
                        logger.Log("Acquiring a security token to call Graph failed. Please try later");
                        break;
                }
                graphResult = null;
            }
            catch (MsalException)
            {
                // For memory, we need to revisit this
                // There will be an Http timeout exception in MSAL 3.0
                graphResult = null;
            }
            var result = await ProcessGraphResultAsync(stateToken, graphResult);
            return new DeltaQueryResult(result);
        }

        /// <summary>
        ///Private function to help with processing of the results of the Graph query
        /// </summary>
        /// <param name="graphResult">The result of the Graph call task</param>
        /// <param name="stateToken">
        /// Skip token returned by a previous call to the service or <see langref="null"/>.
        /// </param>
        /// <returns>Dictionary of the processed results</returns>
        private async Task<Dictionary<string, object>> ProcessGraphResultAsync(string stateToken, IMailFolderDeltaCollectionPage graphResult)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (graphResult != null)
            {
                List<Dictionary<string, object>> resultObjectList = new List<Dictionary<string, object>>();

                if (stateToken == null)
                {
                    //There is a no valid state token so this is a new request
                    for (int i = 0; i < graphResult.Count; i++)
                    {
                        Dictionary<string, object> resultObject = new Dictionary<string, object>()
                    {
                        { "id", graphResult[i].Id }
                    };
                        //add to results
                        resultObjectList.Add(resultObject);
                    }

                    //check if there are more pages of data
                    while (graphResult.NextPageRequest != null)
                    {
                        graphResult = await graphResult.NextPageRequest.GetAsync();
                        for (int i = 0; i < graphResult.Count; i++)
                        {
                            Dictionary<string, object> resultObject = new Dictionary<string, object>()
                        {
                            { "id", graphResult[i].Id }
                        };
                            //add to results
                            resultObjectList.Add(resultObject);
                        }
                    }
                }
                else
                {
                    //There is a valid state token to check for query changes
                    graphResult.InitializeNextPageRequest(graphServiceClient, stateToken);
                    graphResult = await graphResult.NextPageRequest.GetAsync();
                    for (int i = 0; i < graphResult.Count; i++)
                    {
                        Dictionary<string, object> resultObject = new Dictionary<string, object>()
                    {
                        { "id", graphResult[i].Id }
                    };


                        if (graphResult[i].AdditionalData != null)
                        {
                            object removedReason;
                            if (graphResult[i].AdditionalData.TryGetValue("@removed", out removedReason))
                            {
                                resultObject.Add("@removed", removedReason);
                            }
                        }
                        //add to results
                        resultObjectList.Add(resultObject);
                    }
                }

                object deltaLink;
                if (graphResult.AdditionalData.TryGetValue(Constants.DeltaLinkFeedAnnotation, out deltaLink))
                {
                    result.Add(Constants.DeltaLinkFeedAnnotation, deltaLink);
                }

                result.Add("value", resultObjectList.ToArray<Dictionary<string, object>>());
            }
            return result;
        }

        #region helpers   

        /// <summary>
        /// Returns a valid IAuthenticationProvider object to be used for creating a GraphClient
        /// </summary>
        private IAuthenticationProvider GetAuthorizationProvider()
        {
            PublicClientApplication clientApplication = new PublicClientApplication(clientId, authority);
            return new MsalAuthenticationProvider(clientApplication, scopes); ;
        }

        #endregion
    }
}
