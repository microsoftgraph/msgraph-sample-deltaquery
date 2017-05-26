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
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web;
    using System.Web.Script.Serialization;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// Sample implementation of obtaining changes from graph using Delta Query.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// JavaScript Serializer.
        /// </summary>
        private static readonly JavaScriptSerializer _javascriptSerializer = new JavaScriptSerializer();

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
            string appPrincipalPassword,
            ILogger logger)
        {
            this.ReadConfiguration();
            this.tenantDomainName = tenantDomainName;
            this.appPrincipalId = appPrincipalId;
            this.appPrincipalPassword = appPrincipalPassword;
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
        /// Gets or sets the service principal password for your application.
        /// </summary>
        private string appPrincipalPassword { get; set; }

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
            WebClient webClient = new WebClient();

            if (propertyList.Any())
            {
                webClient.QueryString.Add("$select", String.Join(",", propertyList));
            }

            byte[] responseBytes = null;

            this.InvokeOperation(
                () => { responseBytes = DownloadData(webClient, entitySet, stateToken); });

            if (responseBytes != null)
            {
                return new DeltaQueryResult(
                    _javascriptSerializer.DeserializeObject(
                        Encoding.UTF8.GetString(responseBytes)) as Dictionary<string, object>);
            }

            return null;
        }

        #region helpers
        /// <summary>
        /// Get Token for Application.
        /// </summary>
        /// <returns>Token for application.</returns>
        protected string GetTokenForApplication()
        {
            string authEndpoint = string.Format(Constants.AuthEndpoint, this.tenantDomainName);
            AuthenticationContext authenticationContext = new AuthenticationContext(authEndpoint, false);
            
            // Config for OAuth client credentials 
            ClientCredential clientCred = new ClientCredential(this.appPrincipalId, this.appPrincipalPassword);
            AuthenticationResult authenticationResult = authenticationContext.AcquireToken(this.protectedResourcePrincipalId, clientCred);

            return authenticationResult.AccessToken;
        }

        /// <summary>
        /// Returns a string that can logged given a <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="queryParameters">Query parameters to be logged.</param>
        /// <returns>String to be logged.</returns>
        private static string LogQueryParameters(NameValueCollection queryParameters)
        {
            string logString = string.Empty;
            foreach (string key in queryParameters.AllKeys)
            {
                logString = String.Join("&", logString, String.Join("=", key, queryParameters[key]));
            }

            return logString;
        }

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
        /// Adds the required headers to the specified web client.
        /// </summary>
        /// <param name="webClient">Web client to add the required headers to.</param>
        private void AddHeaders(WebClient webClient)
        {
            webClient.Headers.Add(Constants.HeaderNameAuthorization, this.GetTokenForApplication());
            webClient.Headers.Add(HttpRequestHeader.Accept, "application/json");
        }

        /// <summary>
        /// Constructs the URI with the specified entitySet and downloads it with the specified web client.
        /// </summary>
        /// <param name="webClient">Web client to be used to download the URI.</param>
        /// <param name="entitySet">Entity 
        /// to be used to construct the URI.</param>
        /// <returns>Byte array containing the downloaded URI.</returns>
        private byte[] DownloadData(WebClient webClient, string entitySet , string skiptoken)
        {
            this.AddHeaders(webClient);
            string serviceEndPoint = null;
            if (string.IsNullOrEmpty(skiptoken))
            {
                serviceEndPoint = string.Format(
                @"https://{0}/{1}/{2}/delta",
                this.azureADServiceHost,
                this.apiVersion,
                entitySet);
            }
            else {
                serviceEndPoint = skiptoken;
            }

            // Log the query string and endpoint.
            if (this.logger != null)
            {
                this.logger.LogDebug("Making call to endpoint : {0}", serviceEndPoint);
                this.logger.LogDebug("Query Parameters : {0}", LogQueryParameters(webClient.QueryString));
            }

            return webClient.DownloadData(serviceEndPoint);
        }

        /// <summary>
        /// Delegate to invoke the specified operation.
        /// </summary>
        /// <param name="operation">Operation to invoke.</param>
        private void InvokeOperation(Action operation)
        {
            // - retry mechanism to be implemented
            //
            operation();
        }

        /// <summary>
        /// Access token format.
        /// </summary>
        [DataContract]
        private class AccessTokenFormat
        {
            /// <summary>
            /// Gets or sets the token type.
            /// </summary>
            [SuppressMessage(
                "Microsoft.StyleCop.CSharp.NamingRules",
                "SA1300:ElementMustBeginWithUpperCaseLetter",
                Justification = "JSON key name")]
            [DataMember]
            internal string token_type { get; set; }

            /// <summary>
            /// Gets or sets the access token.
            /// </summary>
            [SuppressMessage(
                "Microsoft.StyleCop.CSharp.NamingRules",
                "SA1300:ElementMustBeginWithUpperCaseLetter",
                Justification = "JSON key name")]
            [DataMember]
            internal string access_token { get; set; }
        }

        #endregion
    }
}
