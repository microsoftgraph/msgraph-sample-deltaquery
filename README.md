# ConsoleApp-MicrosoftGraphAPI-DeltaQuery-DotNet

This console application demonstrates how to make Delta Query calls to the Graph API, allowing applications to request only changed data from Microsoft Graph tenants.

The sample uses a application-only permission, however delegated-permissions should also work.


## How To Run This Sample

To run this sample you will need:
- Visual Studio 2015
- .Net 4.5
- An Internet connection
- An Azure Active Directory (Azure AD) tenant. For more information on how to get an Azure AD tenant, please see [How to get an Azure AD tenant](https://azure.microsoft.com/en-us/documentation/articles/active-directory-howto-tenant/) 

### Step 1:  Clone or download this repository

From your shell or command line:

`git clone https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet`

### Step 2:  Running this application with your Azure Active Directory tenant

#### Register Sample app for your own tenant

Using MSAL
1.Before you can get a token from Azure AD v2.0 or Azure AD B2C, you'll need to register an application(https://apps.dev.microsoft.com/). For Azure AD v2.0, use the app registration portal. For Azure AD B2C, checkout how to register your app with B2C.
2. Find the Application ID value and copy it to the clipboard.
3. Configure Permissions for your application - select 'Read Directory Data'.
4. Set a platform by clicking Add Platform, select Native.


###  Step 3: Setup sample project

1. In Solution Explorer, select the **App.config** project.

	a. For the `AppPrincipalId` key, replace `To be filled in` with the application ID of your registered Azure application.
	
	b. For the `AppPrincipalPassword` key, replace `To be filled in` with the key of your registered Azure application.  
	
	c. For the `TenantDomainName` key, replace `To be filled in` with domain name of your organization.

    d. For the `EntitySet` key , you can use delta query supported MS-graph entities e.g `users` ,`groups` etc. as value.

<a name="contributing"></a>
## Contributing ##

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Questions and comments

We'd love to get your feedback about the Microsoft Graph Webhooks sample using WebJobs SDK. You can send your questions and suggestions to us in the [Issues](https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet/issues) section of this repository.

Questions about Microsoft Graph in general should be posted to [Stack Overflow](https://stackoverflow.com/questions/tagged/MicrosoftGraph). Make sure that your questions or comments are tagged with *[MicrosoftGraph]*.

If you have a feature suggestion, please post your idea on our [User Voice](https://officespdev.uservoice.com/) page, and vote for your suggestions there.

## Additional resources

* [AAD DQ sample](https://github.com/Azure-Samples/active-directory-dotnet-graphapi-diffquery)
* [Working with Delta Query in Microsoft Graph](https://developer.microsoft.com/en-us/graph/docs/concepts/delta_query_overview)
* [Microsoft Graph developer site](https://developer.microsoft.com/en-us/graph/)
* [Call Microsoft Graph in an ASP.NET MVC app](https://developer.microsoft.com/en-us/graph/docs/platform/aspnetmvc)
* [MSAL Sample](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet)

Copyright (c) 2017 Microsoft Corporation. All rights reserved.