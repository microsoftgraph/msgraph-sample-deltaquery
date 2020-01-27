// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using DeltaQuery.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System;
using System.Threading.Tasks;

namespace DeltaQuery
{
    class Program
    {
        // The Microsoft Graph permission scopes used by the app
        static string[] _scopes = { "User.Read", "Mail.Read" };

        // The number of seconds to wait between delta queries
        static int _pollIntervalInSecs = 30;
        static GraphServiceClient _graphClient;

        static async Task Main(string[] args)
        {
            var appConfig = LoadAppSettings();

            var authProvider = new DeviceCodeAuthProvider(
                appConfig["AzureAppId"], _scopes);

            _graphClient = new GraphServiceClient(authProvider);

            await WatchMailFolders(_pollIntervalInSecs);
        }

        static async Task WatchMailFolders(int pollInterval)
        {
            // Get first page of mail folders
            IMailFolderDeltaCollectionPage deltaCollection;
            deltaCollection = await _graphClient.Me.MailFolders
                .Delta()
                .Request()
                .GetAsync();

            // TODO: Keep an in-memory dictionary of folder names and ids, use this to give more info
            // Example: "Folder test renamed to test2" or "Folder test2 deleted" or "Folder foo moved to deleted items"
            while(true)
            {
                if (deltaCollection.CurrentPage.Count <= 0)
                {
                    Console.WriteLine("No changes...");
                }
                else
                {
                    bool morePagesAvailable = false;
                    do
                    {
                        morePagesAvailable = deltaCollection.NextPageRequest != null;
                        foreach(var mailFolder in deltaCollection.CurrentPage)
                        {
                            bool isDeleted = mailFolder.AdditionalData != null ?
                                mailFolder.AdditionalData.ContainsKey("@removed") :
                                false;

                            Console.WriteLine($"Folder {mailFolder.DisplayName} {(isDeleted ? "deleted" : "created/updated")}");
                        }

                        if (morePagesAvailable)
                        {
                            deltaCollection = await deltaCollection.NextPageRequest.GetAsync();
                        }
                    }
                    while (morePagesAvailable);
                }

                var deltaLink = deltaCollection.AdditionalData["@odata.deltaLink"];
                if (!string.IsNullOrEmpty(deltaLink.ToString()))
                {
                    Console.WriteLine($"Processed current delta. Will check back in {pollInterval} seconds.");
                    await Task.Delay(pollInterval * 1000);

                    deltaCollection.InitializeNextPageRequest(_graphClient, deltaLink.ToString());
                    deltaCollection = await deltaCollection.NextPageRequest.GetAsync();
                }
            }
        }

        static IConfigurationRoot LoadAppSettings()
        {
            // Load the values stored in the secret
            // manager
            var appConfig = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            // Check for required settings
            if (string.IsNullOrEmpty(appConfig["AzureAppId"]))
            {
                return null;
            }

            return appConfig;
        }
    }
}
