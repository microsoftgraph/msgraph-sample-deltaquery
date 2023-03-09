// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Graph.Me.MailFolders.Delta;

Console.WriteLine("Delta Query Sample app\n");

try
{
    // Load settings from appsettings.json
    var settings = Settings.LoadSettings();

    var pollInterval = settings.PollInterval > 0 ? settings.PollInterval : 30;

    var credentialOptions = await GetCredentialOptionsAsync(settings);
    var credential = new DeviceCodeCredential(credentialOptions);
    if (credentialOptions.AuthenticationRecord == null)
    {
        // Pre-authenticate to persist an auth record
        await PreAuthenticateAsync(credential, settings);
    }

    var graphClient = new GraphServiceClient(credential, settings.GraphUserScopes);

    await WatchMailFoldersAsync(graphClient, pollInterval);
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
    if (ex is ODataError oDataError)
    {
        Console.WriteLine(oDataError.Error?.Message);
    }
    Console.WriteLine(ex.StackTrace);
}

/// <summary>
/// Syncs user's mail folders and periodically polls for changes via delta query.
/// </summary>
/// <param name="graphClient">The GraphServiceClient to use for making API calls</param>
/// <param name="pollInterval">The number of seconds to wait between poll requests</param>
/// <returns>Completed Task</returns>
static async Task WatchMailFoldersAsync(GraphServiceClient graphClient, int pollInterval)
{
    // Local list of mail folders
    var localMailFolders = new List<MailFolder>();

    // Get the first page of folders
    var mailFoldersDelta = await graphClient.Me
        .MailFolders
        .Delta
        .GetAsync();

    while (mailFoldersDelta != null)
    {
        if (mailFoldersDelta.Value == null || mailFoldersDelta.Value.Count <= 0)
        {
            Console.WriteLine("No changes...");
        }
        else
        {
            var morePagesAvailable = false;

            do
            {
                if (mailFoldersDelta == null || mailFoldersDelta.Value == null)
                {
                    continue;
                }

                // Process current page
                foreach (var folder in mailFoldersDelta.Value)
                {
                    await ProcessFolderAsync(graphClient, folder, localMailFolders);
                }

                morePagesAvailable = !string.IsNullOrEmpty(mailFoldersDelta.OdataNextLink);

                if (morePagesAvailable)
                {
                    // If there is a OdataNextLink, there are more pages
                    // Get the next page of results
                    var request = new DeltaRequestBuilder(mailFoldersDelta.OdataNextLink, graphClient.RequestAdapter);
                    mailFoldersDelta = await request.GetAsync();
                }
            }
            while (morePagesAvailable);
        }

        Console.WriteLine($"Processed current delta. Will check back in {pollInterval} seconds.");

        // Once we've iterated through all of the pages, there should
        // be a delta link, which is used to request all changes since our last query
        var deltaLink = mailFoldersDelta?.OdataDeltaLink;
        if (!string.IsNullOrEmpty(deltaLink))
        {
            await Task.Delay(pollInterval * 1000);
            var request = new DeltaRequestBuilder(deltaLink, graphClient.RequestAdapter);
            mailFoldersDelta = await request.GetAsync();
        }
        else
        {
            Console.WriteLine("No @odata.deltaLink found in response!");
        }
    }
}

/// <summary>
/// Checks a mail folder returned from a delta query against the local cache of mail folders.
/// Determines if the folder was added, updated, or deleted.
/// </summary>
/// <param name="graphClient">The GraphServiceClient to use for making API calls</param>
/// <param name="mailFolder">The mail folder</param>
/// <param name="localFolders">The local cache of mail folders</param>
/// <returns>Completed Task</returns>
static async Task ProcessFolderAsync(GraphServiceClient graphClient, MailFolder mailFolder, List<MailFolder> localFolders)
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
                    .GetAsync();

                Console.WriteLine($"  - Moved to {parent?.DisplayName} folder");
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

/// <summary>
/// Generates a DeviceCodeCredentialOptions instance based on application settings.
/// </summary>
/// <param name="settings">The Settings instance containing application settings</param>
/// <returns>DeviceCodeCredentialOptions</returns>
static async Task<DeviceCodeCredentialOptions> GetCredentialOptionsAsync(Settings settings)
{
    var credentialOptions = new DeviceCodeCredentialOptions
    {
        ClientId = settings.ClientId,
        TenantId = settings.TenantId,
        DeviceCodeCallback = (info, cancel) =>
        {
            Console.WriteLine(info.Message);
            return Task.FromResult(0);
        },
        // Set TokenCachePersistenceOptions so Azure Identity library
        // will create a token cache. This will allow subsequent
        // runs of the app to silently authenticate
        TokenCachePersistenceOptions = new TokenCachePersistenceOptions
        {
            Name = "msgraph-sample-deltaquery.cache"
        }
    };

    // In order to silently authenticate, we need to provide an
    // authentication record that identifies the user.
    if (!string.IsNullOrEmpty(settings.AuthRecordCachePath))
    {
        try
        {
            // Attempt to load the cached auth record
            using (var readCacheStream = new FileStream(settings.AuthRecordCachePath, FileMode.Open, FileAccess.Read))
            {
                var authRecord = await AuthenticationRecord.DeserializeAsync(readCacheStream);
                credentialOptions.AuthenticationRecord = authRecord;
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("No cached authentication found");
        }
    }

    return credentialOptions;
}

/// <summary>
/// Authenticates the user and caches the resulting authentication record.
/// </summary>
/// <param name="credential">The DeviceCodeCredential instance to use for authentication</param>
/// <param name="settings">The Settings instance containing application settings</param>
/// <returns>Completed Task</returns>
static async Task PreAuthenticateAsync(DeviceCodeCredential credential, Settings settings)
{
    // If no path is set in settings, there's no need to
    // pre-authenticate
    if (!string.IsNullOrEmpty(settings.AuthRecordCachePath))
    {
        var tokenContext = new TokenRequestContext(settings.GraphUserScopes ??
            new[] {"https://graph.microsoft.com/.default"});
        var authRecord = await credential.AuthenticateAsync(tokenContext);

        if (authRecord != null)
        {
            // Cache the auth record to disk so subsequent runs can
            // use it to silently authenticate
            using (var cacheStream = new FileStream(settings.AuthRecordCachePath, FileMode.Create, FileAccess.Write))
            {
                await authRecord.SerializeAsync(cacheStream);
            }
        }
    }
}
