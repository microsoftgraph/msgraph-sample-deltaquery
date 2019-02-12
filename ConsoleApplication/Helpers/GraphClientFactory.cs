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

using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaQueryApplication
{
    public static class GraphClientFactory
    {
        /// <summary>
        /// Helper to generate an instance of a usable Graphclient class
        /// <param name="clientId">Client id for the application needing to make graph calls</param>
        /// <param name="authority">Url to provide authority for the tenant</param>
        /// <param name="scopes">Scopes to be used by the application </param>
        /// </summary>
        public static GraphServiceClient GetGraphServiceClient(string clientId, string authority, string[] scopes)
        {
            var authenticationProvider = CreateAuthorizationProvider(clientId, authority, scopes);
            return new GraphServiceClient(authenticationProvider);
        }

        /// <summary>
        /// Helper to generate an instance of a IAuthenticationProvider
        /// <param name="clientId">Client id for the application needing to make graph calls</param>
        /// <param name="authority">Url to provide authority for the tenant</param>
        /// <param name="scopes">Scopes to be used by the application </param>
        /// </summary>
        private static IAuthenticationProvider CreateAuthorizationProvider(string clientId, string authority, string[] scopes)
        {
            var clientApplication = new PublicClientApplication(clientId, authority);
            return new MsalAuthenticationProvider(clientApplication, scopes.ToArray());
        }
    }
}
