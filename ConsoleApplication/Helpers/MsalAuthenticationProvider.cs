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

        public MsalAuthenticationProvider(PublicClientApplication clientApplication, IEnumerable<string> scopes) {
            _clientApplication = clientApplication;
            _scopes = scopes;
        }

        /// <summary>
        /// Update HttpRequestMessage with credentials
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