// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using DeltaQuery.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeltaQuery
{
    class Program
    {
        // The Microsoft Graph permission scopes used by the app
        private static string[] _scopes = { "User.Read", "Mail.Read" };

        // The number of seconds to wait between delta queries
        private static int _pollIntervalInSecs = 30;

        // Graph client
        private static GraphServiceClient _graphClient;

        // In-memory "database" of mail folders
        private static List<MailFolder> _localMailFolders = new List<MailFolder>();

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
                        // If there is a NextPageRequest, there are more pages
                        morePagesAvailable = deltaCollection.NextPageRequest != null;
                        foreach(var mailFolder in deltaCollection.CurrentPage)
                        {
                            await ProcessChanges(mailFolder);
                        }

                        if (morePagesAvailable)
                        {
                            // Get the next page of results
                            deltaCollection = await deltaCollection.NextPageRequest.GetAsync();
                        }
                    }
                    while (morePagesAvailable);
                }

                // Once we've iterated through all of the pages, there should
                // be a delta link, which is used to request all changes since our last query
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

        static async Task ProcessChanges(MailFolder mailFolder)
        {
            // Check if the local list of folders already contains this one
            var localFolder = _localMailFolders.Find(f => f.Id == mailFolder.Id);

            bool isDeleted = mailFolder.AdditionalData != null ?
                mailFolder.AdditionalData.ContainsKey("@removed") :
                false;

            if (localFolder != null)
            {
                // In this case it's a delete or an update of a folder
                // we already know about
                if (isDeleted)
                {
                    // Remove the entry from the local list
                    Console.WriteLine($"Folder {localFolder.DisplayName} deleted");
                    _localMailFolders.Remove(localFolder);
                }
                else
                {
                    Console.WriteLine($"Folder {localFolder.DisplayName} updated:");

                    // Was it renamed?
                    if (string.Compare(localFolder.DisplayName, mailFolder.DisplayName) != 0)
                    {
                        Console.WriteLine($"  - Renamed to {mailFolder.DisplayName}");
                    }

                    // Was it moved?
                    if (string.Compare(localFolder.ParentFolderId, mailFolder.ParentFolderId) != 0)
                    {
                        // Get the parent folder
                        var parent = await _graphClient.Me
                            .MailFolders[mailFolder.ParentFolderId]
                            .Request()
                            .GetAsync();

                        Console.WriteLine($"  - Moved to {parent.DisplayName} folder");
                    }

                    // Remove old entry and add new one
                    _localMailFolders.Remove(localFolder);
                    _localMailFolders.Add(mailFolder);
                }
            }
            else
            {
                // No local match
                if (isDeleted)
                {
                    // Folder deleted, but we never knew about it anyway
                    Console.WriteLine($"Unknown folder with ID {mailFolder.Id} deleted");
                }
                else
                {
                    // New folder, add to local list
                    Console.WriteLine($"Folder {mailFolder.DisplayName} added");
                    _localMailFolders.Add(mailFolder);
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
