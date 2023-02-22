// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Identity;
using Microsoft.Graph;

Console.WriteLine("Delta Query Sample app\n");

try
{
    // Load settings from appsettings.json
    var settings = Settings.LoadSettings();

    var pollInterval = settings.PollInterval > 0 ? settings.PollInterval : 30;

    var credential = new DeviceCodeCredential(
        (info, cancel) =>
        {
            Console.WriteLine(info.Message);
            return Task.FromResult(0);
        },
        settings.TenantId,
        settings.ClientId
    );

    var graphClient = new GraphServiceClient(credential, settings.GraphUserScopes);

    await WatchMailFolders(graphClient, pollInterval);
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}

static async Task WatchMailFolders(GraphServiceClient graphClient, int pollInterval)
{
    // Local list of mail folders
    var localMailFolders = new List<MailFolder>();

    // Get the first page of folders
    var mailFoldersDelta = await graphClient.Me
        .MailFolders
        .Delta()
        .Request()
        .GetAsync();

    while (true)
    {
        if (mailFoldersDelta.CurrentPage.Count <= 0)
        {
            Console.WriteLine("No changes...");
        }
        else
        {
            var morePagesAvailable = false;

            do
            {
                // Process current page
                foreach (var folder in mailFoldersDelta.CurrentPage)
                {
                    await ProcessFolder(graphClient, folder, localMailFolders);
                }

                morePagesAvailable = mailFoldersDelta.NextPageRequest != null;

                if (mailFoldersDelta.NextPageRequest != null)
                {
                    // If there is a NextPageRequest, there are more pages
                    // Get the next page of results
                    mailFoldersDelta = await mailFoldersDelta.NextPageRequest.GetAsync();
                }
            }
            while (morePagesAvailable);
        }

        Console.WriteLine($"Processed current delta. Will check back in {pollInterval} seconds.");

        // Once we've iterated through all of the pages, there should
        // be a delta link, which is used to request all changes since our last query
        var deltaLink = mailFoldersDelta.AdditionalData["@odata.deltaLink"].ToString();
        if (!string.IsNullOrEmpty(deltaLink))
        {
            await Task.Delay(pollInterval * 1000);

            mailFoldersDelta.InitializeNextPageRequest(graphClient, deltaLink);
            if (mailFoldersDelta.NextPageRequest != null)
            {
                mailFoldersDelta = await mailFoldersDelta.NextPageRequest.GetAsync();
            }
        }
        else
        {
            Console.WriteLine("No @odata.deltaLink found in response!");
        }
    }
}

static async Task ProcessFolder(GraphServiceClient graphClient, MailFolder mailFolder, List<MailFolder> localFolders)
{
    // Check if the local list of folders already contains this one
    var localFolder = localFolders.Find(f => f.Id == mailFolder.Id);

    var isDeleted = mailFolder.AdditionalData != null ?
        mailFolder.AdditionalData.ContainsKey("@removed") :
        false;

    if (localFolder != null)
    {
        // In this case it's a delete or an update of
        // a folder we already know about
        if (isDeleted)
        {
            // Remove the entry from the local list
            Console.WriteLine($"Folder {localFolder.DisplayName} deleted");
            localFolders.Remove(localFolder);
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
                var parent = await graphClient.Me
                    .MailFolders[mailFolder.ParentFolderId]
                    .Request()
                    .GetAsync();

                Console.WriteLine($"  - Moved to {parent.DisplayName} folder");
            }

            // Remove old entry and add new one
            localFolders.Remove(localFolder);
            localFolders.Add(mailFolder);
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
            localFolders.Add(mailFolder);
        }
    }
}
