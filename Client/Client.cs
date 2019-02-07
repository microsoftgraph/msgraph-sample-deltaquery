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
    using System.Net;
    using Microsoft.Graph;

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
        /// <param name="tenantDomainName">Windows Azure AD tenant domain name.</param>
        /// <param name="appPrincipalId">Service principal ID.</param>
        /// <param name="appPrincipalPassword">Service principal password.</param>
        /// <param name="logger">Logger to be used for logging output/debug.</param>
        /// <param name="authToken"></param>
        public Client(
            string tenantDomainName,
            string appPrincipalId,
            ILogger logger)
        {
            this.ReadConfiguration();
            this.tenantDomainName = tenantDomainName;
            this.appPrincipalId = appPrincipalId;
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets the Graph service endpoint.
        /// </summary>
        protected string azureADServiceHost { get; set; }

        /// <summary>
        /// Gets or sets the Graph API version.
        /// </summary>
        protected string apiVersion { get; set; }

        /// <summary>
        /// Gets or sets the well known service principal ID for Windows Azure AD Access Control.
        /// </summary>
        private string protectedResourcePrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the Windows Azure AD tenant domain name.
        /// </summary>
        private string tenantDomainName { get; set; }

        /// <summary>
        /// Gets or sets the service principal ID for your application.
        /// </summary>
        private string appPrincipalId { get; set; }

        /// <summary>
        /// Calls the Delta Query service and returns the result.
        /// </summary>
        /// <param name="stateToken">
        /// Skip token returned by a previous call to the service or <see langref="null"/>.
        /// </param>
        /// <returns>Result from the Delta Query service.</returns>
        public DeltaQueryResult DeltaQuery(string stateToken, string entitySet)
        {
            return this.DeltaQuery(
                entitySet,
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
        public DeltaQueryResult DeltaQuery(
            string entitySet,
            string stateToken,
            ICollection<string> propertyList)
        {

            List<QueryOption> options = new List<QueryOption>();
            if (propertyList.Any())
            {
                foreach (string parameter in propertyList)
                {
                   options.Add( new QueryOption("$select", parameter));
                }
            }

            String[] scopes = new[] { "Mail.Read" };
            var authenticationProvider = GetAuthorizationProvider(scopes);
            GraphServiceClient myClient = new GraphServiceClient(authenticationProvider);
            var graphResult = myClient.Me.MailFolders.Delta().Request(options).GetAsync().Result;
            List<object> resultObjectList = new List<object>();

            if (stateToken == null)
            {
                //There is a no valid state so this is a new request
                for (int i = 0; i < graphResult.Count; i++)
                {
                    Dictionary<string, object> resultObject = new Dictionary<string, object>();
                    resultObject.Add("id", graphResult[i].Id);
                    resultObjectList.Add(resultObject);
                }

                //check if there are more pages of data
                while (graphResult.NextPageRequest != null)
                {
                    graphResult = graphResult.NextPageRequest.GetAsync().Result;
                    for (int i = 0; i < graphResult.Count; i++)
                    {
                        Dictionary<string, object> resultObject = new Dictionary<string, object>();
                        resultObject.Add("id", graphResult[i].Id);
                        resultObjectList.Add(resultObject);
                    }
                }
            }
            else
            {
                //There is a valid state token to check for query changes
                graphResult.InitializeNextPageRequest(myClient,stateToken);
                graphResult = graphResult.NextPageRequest.GetAsync().Result;
                for (int i = 0; i < graphResult.Count; i++)
                {
                    Dictionary<string, object> resultObject = new Dictionary<string, object>();
                    resultObject.Add("id", graphResult[i].Id);
                    object removedReason;
                    if (graphResult.AdditionalData.TryGetValue("@removed", out removedReason))
                    {
                        resultObject.Add("@removed", removedReason);
                    }
                    resultObjectList.Add(resultObject);
                }
            }

            object deltaLink;
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (graphResult.AdditionalData.TryGetValue(Constants.DeltaLinkFeedAnnotation, out deltaLink))
            {
                result.Add(Constants.DeltaLinkFeedAnnotation,deltaLink);
            }
            
            result.Add("value", resultObjectList.ToArray<object>());

            return new DeltaQueryResult(result);
           
        }

        #region helpers   

        /// <summary>
        /// Reads the client configuration.
        /// </summary>
        private void ReadConfiguration()
        {
            this.azureADServiceHost = Configuration.GetElementValue("AzureADServiceHost");
            this.apiVersion = Configuration.GetElementValue("ApiVersion");
            this.protectedResourcePrincipalId = Configuration.GetElementValue("ProtectedResourcePrincipalId");
        }

        /// <summary>
        /// Returns a valid IAuthenticationProvider object to be used for creating a GraphClient
        /// </summary>
        private IAuthenticationProvider GetAuthorizationProvider(String [] scopes)
        {
            String authority = "https://login.microsoftonline.com/common";
            PublicClientApplication clientApplication = new PublicClientApplication(this.appPrincipalId, authority);
            return new MsalAuthenticationProvider(clientApplication, scopes); ;
        }

        #endregion
    }
}
