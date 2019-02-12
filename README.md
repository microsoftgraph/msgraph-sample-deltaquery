# ConsoleApp-MicrosoftGraphAPI-DeltaQuery-DotNet

This console application demonstrates how to make Delta Query calls to the Graph API, allowing applications to request only changed data from Microsoft Graph tenants.

The sample uses demonstates how graph calls can be made with the Graph SDK and how the reponses can be handled.

The specific example used in this sample involves monitoring changes(addition and removal) of MailFolders in an individual's email account.

## How To Run This Sample

To run this sample you will need:
- Visual Studio 2017
- An Internet connection
- An Azure Active Directory (Azure AD) tenant. For more information on how to get an Azure AD tenant, please see [How to get an Azure AD tenant](https://azure.microsoft.com/en-us/documentation/articles/active-directory-howto-tenant/) 

### Step 1:  Clone or download this repository

From your shell or command line:

`git clone https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet`

### Step 2:  Configuring your Azure AD tenant

As a first step you'll need to:

1. Sign in to the [Azure portal](https://portal.azure.com) using either a work or school account or a personal Microsoft account.
1. If your account gives you access to more than one tenant, select your account in the top right corner, and set your portal session to the desired Azure AD tenant
   (using **Switch Directory**).
1. In the left-hand navigation pane, select the **Azure Active Directory** service, and then select **App registrations (Preview)**.

#### Register the client app (ConsoleApp-DeltaQuery-DotNet)

1. In **App registrations (Preview)** page, select **Register an Application**.
1. When the **Register an application page** appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `ConsoleApp-DeltaQuery-DotNet`.
   - In the **Supported account types** section, select **Accounts in any organizational directory and personal Microsoft accounts (e.g. Skype, Xbox, Outlook.com)**.
   - Select **Register** to create the application.
1. On the app **Overview** page, find the **Application (client) ID** value and record it for later. You'll need it to configure the Visual Studio configuration file for this project.
1. In the list of pages for the app, select **Manifest**, and:
   - In the manifest editor, set the ``allowPublicClient`` property to **true**
   - Select **Save** in the bar above the manifest editor.
1. In the list of pages for the app, select **Authentication**
   - In the *Suggested Redirect URIs for public clients(mobile,desktop)*, check all the boxes so that the app can work with the MSAL libs used in the application.
1. In the list of pages for the app, select **API permissions**
   - Click the **Add a permission** button and then,
   - Ensure that the **Microsoft APIs** tab is selected
   - In the *Commonly used Microsoft APIs* section, click on **Microsoft Graph**
   - In the **Delegated permissions** section, ensure that the right permissions are checked: **Mail.Read**. Use the search box if necessary.
   - Select the **Add permissions** button

### Step 3:  Configure the sample to use your Azure AD tenant

In the steps below, "ClientID" is the same as "Application ID" or "AppId".

Open the solution in Visual Studio to configure the projects

#### Configure the client project

1. Open the `ConsoleApplication\appsettings.json` file
1. Find the line where `ClientId` is set and replace the existing value with the application ID (clientId) of the `ConsoleApp-DeltaQuery-DotNet` application copied from the Azure portal.
1. [optionally] Find the line where `TenantId` is set and replace the existing value with your tenant ID.

Clean the solution, rebuild the solution, and start it in the debugger.

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Questions and comments

We'd love to get your feedback about the Microsoft Graph Webhooks sample using WebJobs SDK. You can send your questions and suggestions to us in the [Issues](https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet/issues) section of this repository.

Questions about Microsoft Graph in general should be posted to [Stack Overflow](https://stackoverflow.com/questions/tagged/MicrosoftGraph). Make sure that your questions or comments are tagged with *[MicrosoftGraph]*.

If you have a feature suggestion, please post your idea on our [User Voice](https://officespdev.uservoice.com/) page, and vote for your suggestions there.

## Additional resources

- [AAD DQ sample](https://github.com/Azure-Samples/active-directory-dotnet-graphapi-diffquery)
- [Working with Delta Query in Microsoft Graph](https://developer.microsoft.com/en-us/graph/docs/concepts/delta_query_overview)
- [Microsoft Graph developer site](https://developer.microsoft.com/en-us/graph/)
- [Call Microsoft Graph in an ASP.NET MVC app](https://developer.microsoft.com/en-us/graph/docs/platform/aspnetmvc)
- [MSAL Sample](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet)

Copyright (c) 2019 Microsoft Corporation. All rights reserved.