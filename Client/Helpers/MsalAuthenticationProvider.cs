using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using System.Collections.Generic;

namespace DeltaQueryClient
{
    // This class encapsulates the details of getting a token from MSAL and exposes it via the 
    // IAuthenticationProvider interface so that GraphServiceClient or AuthHandler can use it.
    // A significantly enhanced version of this class will in the future be available from
    // the GraphSDK team.  It will supports all the types of Client Application as defined by MSAL.
    public class MsalAuthenticationProvider : IAuthenticationProvider
    {
        private PublicClientApplication _clientApplication;
        private IEnumerable<string> _scopes;
        private IAccount _account = null;

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
            if (_account != null)
            {
                try
                {
                    authResult = await _clientApplication.AcquireTokenSilentAsync(_scopes, _account);
                }
                catch (MsalUiRequiredException)
                {
                    authResult = await _clientApplication.AcquireTokenAsync(_scopes);
                    _account = authResult.Account;
                }
            }
            else
            {
                authResult = await _clientApplication.AcquireTokenAsync(_scopes);
                _account = authResult.Account;
            }

            return authResult.AccessToken;
        }
    }
}