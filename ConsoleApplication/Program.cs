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

namespace DeltaQueryApplication
{
    using System.Configuration;
    using System.Threading.Tasks;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;

    /// <summary>
    /// Delta Query sample console application
    /// </summary>
    public class Program
    {

        private static GraphServiceClient _graphServiceClient;

        /// <summary>
        /// Application entry point 
        /// </summary>
        private static void Main()
        {
            AppConfiguration appConfiguration = AppConfiguration.ReadFromJsonFile("appsettings.json");

            _graphServiceClient =
               GraphClientFactory.GetGraphServiceClient(
                    appConfiguration.ClientId,
                    appConfiguration.Authority,
                    new string[] { Constants.DefaultScope });

            // Start watching mail folders
            try
            {
                Task.Run(() => WatchMailFolders(appConfiguration.PullIntervalSec)).Wait();
            }
            catch (MsalServiceException mse)
            {
                ProcessMsalException(mse);
            }
            catch (MsalException me)
            {
                // For memory, we need to revisit this
                // There will be an Http timeout exception in MSAL 3.0
                Console.WriteLine(me.Message);
                Console.WriteLine("Error occured (MsalException) with Graph call");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Method for watching for changes in MailFolders
        /// </summary>
        /// <param name="pullIntervalSec"></param>
        /// <returns></returns>
        public static async Task WatchMailFolders(int pullIntervalSec)
        {
            // Retreive first page of mailfolders
            IMailFolderDeltaCollectionPage deltaCollection;
            deltaCollection = await _graphServiceClient.Me.MailFolders.Delta().Request().GetAsync();
            
            while (true)
            {
                // Iterate over pages of mailfolders and record DeltaLink in last page
                var changedMailFolders = new List<MailFolder>();
                var iterator = new PageIterator(deltaCollection, (m)=> changedMailFolders.Add(m));
                await iterator.Iterate();

                if (changedMailFolders.Count == 0)
                {
                    Console.WriteLine("No changes");
                }
                else
                {
                    foreach (MailFolder mailfolder in changedMailFolders)
                    {
                        string mailfolderName = mailfolder.DisplayName;
                        bool isDeleted = mailfolder.AdditionalData != null ? mailfolder.AdditionalData.ContainsKey("@removed") : false;
                        if (isDeleted)
                        {
                            Console.WriteLine($"Object {mailfolderName} deleted");  // Interactively deleting MailFolders actually only moves them to DeletedItems so they show as "updated".
                        }
                        else
                        {
                            Console.WriteLine($"Object {mailfolderName} created/updated");
                        }
                    }
                }

                // Delta query should return DeltaLink for us to make another query in the future.
                if (iterator.DeltaLink != null)
                {
                    Console.WriteLine(
                        "Processed change(s) successfully. Will check back in {0} sec.",
                        pullIntervalSec);
                    await Task.Delay(pullIntervalSec * 1000);

                    // Request Mailfolders changed since last call
                    deltaCollection.InitializeNextPageRequest(_graphServiceClient, iterator.DeltaLink);
                    deltaCollection = await deltaCollection.NextPageRequest.GetAsync();
                }
            }
        }

        /// <summary>
        /// Method for handling exceptions thrown by the MSAL library.
        /// </summary>
        /// <param name="mse">Intance of an excpetion of the type <see cref="MsalServiceException"/>> </param>
        /// <returns></returns>
        private static void ProcessMsalException(MsalServiceException mse)
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
                    Console.WriteLine("The application is not configured correctly with Azure AD");
                    break;
                case MsalServiceException.RequestTimeout:
                case MsalServiceException.ServiceNotAvailable:
                    Console.WriteLine("Acquiring a security token to call Graph failed. Please try later");
                    break;
                default:
                    Console.WriteLine(mse.Message);
                    Console.WriteLine("Error occured with Graph call");
                    break;
            }
        }
    }
}
