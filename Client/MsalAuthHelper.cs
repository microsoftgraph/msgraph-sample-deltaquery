//-------------------------------------------------------------------------------------------------
// <copyright file="ChangeManager.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     Delta Query sample application.
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
//

namespace DeltaQueryClient
{
    using Microsoft.Identity.Client;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    class MsalAuthHelper

    {
        private string _clientId;
        public PublicClientApplication Application { get; private set; }

        public MsalAuthHelper(string clientId)
        {
            _clientId = clientId;
            Application = new PublicClientApplication(_clientId, "https://login.microsoftonline.com/common/");
        }

        public async Task<IUser> SignIn()

        {
            try
            {
                AuthenticationResult result = await Application.AcquireTokenAsync(new[] { "Directory.Read.All" }).ConfigureAwait(false);
                return result.User;
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Sign in failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        public async Task<string> GetTokenForCurrentUser(IEnumerable<string> scopes, IUser user)

        {

            AuthenticationResult result = null;
            Exception exception = null;

            try
            {
                result = await Application.AcquireTokenAsync(scopes, user).ConfigureAwait(false);

                return result.AccessToken;
            }

            catch (MsalUiRequiredException exc)

            {
                try
                {
                    result = await Application.AcquireTokenAsync(scopes, user).ConfigureAwait(false);

                    return result.AccessToken;
                }

                catch (Exception ex)
                {

                    exception = ex;
                }
            }

            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception != null)
            {
                MessageBox.Show(exception.Message, "Failed to get token", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }
    }
}
