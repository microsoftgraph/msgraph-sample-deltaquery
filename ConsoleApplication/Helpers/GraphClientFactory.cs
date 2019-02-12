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
        public static GraphServiceClient GetGraphServiceClient(string clientId, string authority, string[] scopes)
        {
            var authenticationProvider = CreateAuthorizationProvider(clientId, authority, scopes);
            return new GraphServiceClient(authenticationProvider);
        }

        private static IAuthenticationProvider CreateAuthorizationProvider(string clientId, string authority, string[] scopes)
        {
            var clientApplication = new PublicClientApplication(clientId, authority);
            return new MsalAuthenticationProvider(clientApplication, scopes.ToArray());
        }
    }
}
