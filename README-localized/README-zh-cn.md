---
page_type: sample
products:
- ms-graph
languages:
- csharp
extensions:
  contentType: samples 
  technologies:
  - Microsoft Graph
  createdDate: 5/25/2017 5:03:53 PM
---
# ConsoleApp-MicrosoftGraphAPI-DeltaQuery-DotNet

此控制台应用程序演示如何对 Graph API 进行 Delta 查询调用，允许应用程序仅从 Microsoft Graph 租户请求更改的数据。

此示例演示如何使用 Graph SDK 调用图像以及如何处理响应。

此示例中使用的指定例子涉及监控各电子邮件账户中 MailFolders 的变化（添加和删除）。

## 如何运行此示例

若要运行此示例，需要：
- Visual Studio 2017
- 网络连接
- Azure Active Directory (Azure AD) 租户。有关如何获取 Azure AD 租户的详细信息，请参阅[如何获取 Azure AD 租户](https://azure.microsoft.com/en-us/documentation/articles/active-directory-howto-tenant/) 

### 步骤 1：克隆或下载此存储库

在 shell 或命令行中键入：

`git clone https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet`

### 步骤 2：将示例注册到 Azure Active Directory 租户

此示例中有一个项目。若要注册，可：

- 遵照 [步骤 2：将示例注册到 Azure Active Directory 租户](#step-2-register-the-sample-with-your-azure-active-directory-tenant)和[步骤 3：将示例配置为使用 Azure AD 租户](#choose-the-azure-ad-tenant-where-you-want-to-create-your-applications)
- 或使用 PowerShell 脚本：
  - **自动**为你创建 Azure AD 应用和相关对象（密码、权限、依赖项）
  - 修改 Visual Studio 项目的配置文件。

如果希望使用此自动化：

1. 在 Windows 上运行 PowerShell 并导航至克隆目录的根
1. 在 PowerShell 中，运行：

   ```PowerShell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force
   ```

1. 运行此脚本创建 Azure AD 应用并相应配置示例应用的代码。

   ```PowerShell
   .\AppCreationScripts\Configure.ps1
   ```

   > 有关运行脚本的其他方法，请参阅“[应用程序创建脚本](./AppCreationScripts/AppCreationScripts.md)”

1. 打开 Visual Studio 解决方案并点击开始

如果不希望使用此自动化，请按照下列步骤进行

#### 选择要在其中创建应用程序的 Azure AD 租户

第一步需要执行以下操作:

1. 使用工作/学校帐户或 Microsoft 个人帐户登录到 [Azure 门户](https://portal.azure.com)。
1. 如果你的帐户有权访问多个租户，请在右上角选择该帐户，并将门户会话设置为所需的 Azure AD 租户（使用“**切换目录**”）。
1. 在左侧导航窗格中选择“**Azure Active Directory**”服务，然后选择“**应用注册(预览版)**”。

#### 注册客户端应用（ConsoleApp-DeltaQuery-DotNet）

1. 在“**应用注册(预览版)**”页面中，选择“**注册应用程序**”。
1. 出现“**注册应用程序页**”后，输入应用程序的注册信息：
   - 在“**名称**”部分输入一个会显示给应用用户的有意义的应用程序名称，例如 `ConsoleApp-DeltaQuery-DotNet`。
   - 在“**支持的帐户类型**”部分，选择“**任何组织目录中的帐户和个人 Microsoft 帐户(例如 Skype、Xbox、Outlook.com)**”。
   - 选择“**注册**”以创建应用程序。
1. 在应用的“**概述**”页上，查找“**应用程序(客户端) ID**”值，并稍后记录下来。你将需要它来为此项目配置 Visual Studio 配置文件。
1. 在应用的页面列表中，选择“**身份验证**”
   - 在“*适用于公共客户端(移动、桌面)的建议重定向 URI*”中，选中第二个框，以便应用可以使用应用程序中使用的 MSAL 库。（此框应包含选项“*urn:ietf:wg:oauth:2.0:oob*”）。 
1. 在应用的页面列表中，选择“**API 权限**”
   - 单击“**添加权限”**按钮，然后，
   - 确保已选中”**Microsoft API**”选项卡
   - 在“*常用 Microsoft API*”部分，单击“**Microsoft Graph**”
   - 在“**委派权限**”部分，确保已选中适当的权限：**Mail.Read**。必要时请使用搜索框。
   - 选择“**添加权限**”按钮

### 步骤 3：将示例配置为使用 Azure AD 租户

在下面的步骤中，“客户端 ID” 与“应用程序 ID” 或“AppId”相同。

在 Visual Studio 中打开解决方案以配置项目

#### 配置客户端项目

1. 在 *ConsoleApplication* 文件夹中，将 `appsettings.json.example` 文件重命名为 `appsettings.json`
1. 打开并编辑 `appsettings.json` 文件以进行以下更改
    1. 找到将 `ClientId` 设置为 `YOUR_CLIENT_ID_HERE` 的行，然后将现有值替换为从 Azure 门户复制的 `ConsoleApp-DeltaQuery-DotNet`应用程序的“应用程序(客户端) ID”。

清除解决方案，重新生成解决方案，并在调试程序中启动。

## 参与

如果想要参与本示例，请参阅 [CONTRIBUTING.MD](/CONTRIBUTING.md)。

此项目已采用 [Microsoft 开放源代码行为准则](https://opensource.microsoft.com/codeofconduct/)。有关详细信息，请参阅[行为准则 FAQ](https://opensource.microsoft.com/codeofconduct/faq/)。如有其他任何问题或意见，也可联系 [opencode@microsoft.com](mailto:opencode@microsoft.com)。

## 问题和意见

我们乐于收到反馈，了解使用 WebJobs SDK 的 Microsoft Graph Webhook 示例的情况。你可通过该存储库中的[问题](https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet/issues)部分向我们发送问题和建议。

与 Microsoft Graph 相关的一般问题应发布到 [Stack Overflow](https://stackoverflow.com/questions/tagged/MicrosoftGraph)。请确保你的问题或意见标记有 *\[MicrosoftGraph]*。

如果有功能建议，请将你的想法发布在我们的 [User Voice](https://officespdev.uservoice.com/) 页上，并为你的建议进行投票。

## 其他资源

- [AAD DQ 示例](https://github.com/Azure-Samples/active-directory-dotnet-graphapi-diffquery)
- [在 Microsoft Graph 中使用 Delta Query](https://developer.microsoft.com/en-us/graph/docs/concepts/delta_query_overview)
- [Microsoft Graph 开发人员网站](https://developer.microsoft.com/en-us/graph/)
- [在 ASP.NET MVC 应用中调用 Microsoft Graph](https://developer.microsoft.com/en-us/graph/docs/platform/aspnetmvc)
- [MSAL.NET](https://aka.ms/msal-net)

版权所有 (c) 2019 Microsoft Corporation。保留所有权利。
