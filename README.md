# ConsoleApp-MicrosoftGraphAPI-DeltaQuery-DotNet

This console application demonstrates how to make Delta Query calls to the Graph API, allowing applications to request only changed data from Microsoft Graph tenants.

The sample uses a application-only permission, however delegated-permissions should also work.


## How To Run This Sample

To run this sample you will need:
- Visual Studio 2015
- An Internet connection
- An Azure Active Directory (Azure AD) tenant. For more information on how to get an Azure AD tenant, please see [How to get an Azure AD tenant](https://azure.microsoft.com/en-us/documentation/articles/active-directory-howto-tenant/) 

### Step 1:  Clone or download this repository

From your shell or command line:

`git clone https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet`

### Step 2:  Running this application with your Azure Active Directory tenant

#### Register Sample app for your own tenant

1. Sign in to the [Azure portal](https://portal.azure.com).
2. On the top bar, click on your account and under the **Directory** list, choose the Active Directory tenant where you wish to register your application.
3. Click on **More Services** in the left hand nav, and choose **Azure Active Directory**.
4. Click on **App registrations** and choose **Add**.
5. Enter a friendly name for the application, for example 'WebApp for Azure AD' and select 'Web Application and/or Web API' as the Application Type. For the sign-on URL, enter the base URL for the sample, which is by default `https://localhost:44322`. Click on **Create** to create the application.
6. While still in the Azure portal, choose your application, click on **Settings** and choose **Properties**.
7. Find the Application ID value and copy it to the clipboard.
8. For the App ID URI, enter `https://<your_tenant_name>/WebAppGraphAPI`, replacing `<your_tenant_name>` with the domain name of your Azure AD tenant. For example "https://contoso.com/WebAppGraphAPI".
9. From the settings page, click on 'Reply URLs' and add the reply URL address used to return the authorization code returned during Authorization code flow.  For example: "https://localhost:44322/".
10. Configure Permissions for your application - in the Settings menu, choose the 'Required permissions' section, click on **Add**, then **Select an API**, and select 'Microsoft Graph' (this is the Graph API). Then, click on  **Select Permissions** and select 'Read Directory Data'.


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

Copyright (c) 2017 Microsoft Corporation. All rights reserved.