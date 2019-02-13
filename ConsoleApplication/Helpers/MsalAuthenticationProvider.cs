/*
 The MIT License (MIT)

Copyright (c) 2019 Microsoft Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;

namespace DeltaQueryApplication
{
    // This class encapsulates the details of getting a token from MSAL and exposes it via the 
    // IAuthenticationProvider interface so that GraphServiceClient or AuthHandler can use it.
    // A significantly enhanced version of this class will in the future be available from
    // the GraphSDK team.  It will supports all the types of Client Application as defined by MSAL.
    public class MsalAuthenticationProvider : IAuthenticationProvider
    {
        private PublicClientApplication _clientApplication;
        private IEnumerable<string> _scopes;

        /// <summary>
        /// Constructor for the MsalAuthenticationProvider
        /// <param name="clientApplication">Client application to be used by the class</param>
        /// <param name="scopes">Scopes to be used by the class </param>
        /// </summary>
        public MsalAuthenticationProvider(PublicClientApplication clientApplication, IEnumerable<string> scopes) {
            _clientApplication = clientApplication;
            _scopes = scopes;
        }

        /// <summary>
        /// Update HttpRequestMessage with credentials
        /// <param name="request">Http request to update with  credentials</param>
        /// </summary>
        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var token = await GetTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        /// <summary>
        /// Acquire Token 
        /// </summary>
        public async Task<string> GetTokenAsync()
        {
            AuthenticationResult authResult = null;
            var accounts = await _clientApplication.GetAccountsAsync();
            IAccount account = accounts.FirstOrDefault(); // Supposing here there is only one user to the app

            try
            {
                // Benefit from the token cache, and automatic token refresh
                authResult = await _clientApplication.AcquireTokenSilentAsync(_scopes, account);
            }
            catch (MsalUiRequiredException)
            {
                authResult = await _clientApplication.AcquireTokenAsync(_scopes);
            }
            return authResult.AccessToken;

        }
    }
}