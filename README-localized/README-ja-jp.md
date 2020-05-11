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

このコンソール アプリケーションは、アプリケーションが Microsoft Graph テナントからの変更データのみを要求できるように、Graph API に対するデルタ クエリ呼び出しを行う方法を示します。

使用するサンプルは、Graph SDK を使用してグラフの呼び出しを行う方法と、応答を処理する方法を示しています。

このサンプルで使用される特定の例には、個人のメール アカウントの MailFolders の変更 (追加および削除) の監視が含まれます。

## このサンプルの実行方法

このサンプルを実行するには、次のものが必要です。
- Visual Studio 2017
- インターネット接続
- Azure Active Directory (Azure AD) テナント。Azure AD テナントを取得する方法の詳細については、「[Azure AD テナントを取得する方法](https://azure.microsoft.com/en-us/documentation/articles/active-directory-howto-tenant/)」を参照してください 

### 手順 1: このリポジトリのクローンを作成するか、ダウンロードします

シェルまたはコマンド ラインから:

`git clone https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet`

### 手順 2:Azure Active Directory テナントにサンプル アプリケーションを登録する

このサンプルには 1 つのプロジェクトがあります。登録するには、次の操作を行います。

- 次の手順に従ってください [手順 2:サンプルを Azure Active Directory テナント](#step-2-register-the-sample-with-your-azure-active-directory-tenant)に登録し、[手順 3:サンプルを構成して Azure AD テナント](#choose-the-azure-ad-tenant-where-you-want-to-create-your-applications)を使用します
- または、[PowerShell スクリプト] を使用して:
  - Azure AD アプリケーションと関連オブジェクト (パスワード、アクセス許可、依存関係) を**自動的に**作成します
  - Visual Studio プロジェクトの構成ファイルを変更します。

このオートメーションを使用する場合:

1. Windows で PowerShell を実行し、複製ディレクトリのルートに移動します
1. PowerShell で、以下を実行します。

   ```PowerShell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force
   ```

1. スクリプトを実行して Azure AD アプリケーションを作成し、サンプル アプリケーションのコードを適切に構成します。

   ```PowerShell
   .\AppCreationScripts\Configure.ps1
   ```

   > スクリプトを実行する他の方法は、「[App Creation Scripts (アプリ作成スクリプト)](./AppCreationScripts/AppCreationScripts.md)」で説明されています

1. Visual Studio ソリューション ファイルを開き、[開始] をクリックします

このオートメーションを使用しない場合は、次の手順に従ってください

#### アプリケーションを作成する Azure AD テナントを選択します

まず、次のことを行う必要があります。

1. 職場または学校のアカウントか、個人の Microsoft アカウントを使用して、[Azure portal](https://portal.azure.com)にサインインします。
1. ご利用のアカウントで複数のテナントにアクセスできる場合は、右上隅でアカウントを選択し、ポータルのセッションを目的の Azure AD テナントに設定します (**Switch Directory** を使用)。
1. 左側のナビゲーション ウィンドウで、[**Azure Active Directory**] サービスを選択し、[**アプリの登録 (プレビュー)**] を選択します。

#### クライアント アプリ (ConsoleApp-DeltaQuery-DotNet) を登録します。

1. [**アプリの登録 (プレビュー)**] ページで、[**アプリケーションを登録する**] を選択します。
1. [**アプリケーションの登録ページ**] が表示されたら、以下のアプリケーションの登録情報を入力します。
   - [**名前**] セクションに、アプリのユーザーに表示されるわかりやすいアプリケーション名を入力します (例: `ConsoleApp-DeltaQuery-DotNet`)。
   - [**サポートされているアカウントの種類**] セクションで、[**組織ディレクトリ内のアカウントと個人の Microsoft アカウント (例: Skype、Xbox、Outlook.com)**] を選択します。
   - [**登録**] を選択して、アプリケーションを作成します。
1. アプリの [**概要**] ページで、[**Application (client) ID**] (アプリケーション (クライアント) ID) の値を確認し、後で使用するために記録します。この情報は、このプロジェクトで Visual Studio 構成ファイルを設定するのに必要になります。
1. アプリのページの一覧から [**認証**] を選択します
   - [*パブリック クライアント (モバイル、デスクトップ) に推奨されるリダイレクト URI*] で、アプリケーションで使用されている MSAL ライブラリをアプリが利用できるように、2 番目のボックスをオンにします。(ボックスには、オプション *urn:ietf:wg:oauth:2.0:oob* を含める必要があります)。 
1. アプリのページの一覧から [**API のアクセス許可**] を選択します。
   - [**アクセス許可の追加]**] ボタンをクリックします
   - [**Microsoft API**] タブが選択されていることを確認します
   - [*一般的に使用される Microsoft API*] セクションで、[**Microsoft Graph**] をクリックします
   - [**委任されたアクセス許可**] セクションで、適切なアクセス許可がチェックされていることを確認します。**Mail.Read**。必要に応じて検索ボックスを使用します。
   - [**アクセス許可の追加**] ボタンを選択します

### 手順 3:Azure AD テナントを使用するようにサンプルを構成する

次の手順では、"ClientID" は "Application ID" または "AppId" と同じです。

Visual Studio でソリューションを開き、プロジェクトを構成します

#### クライアント プロジェクトを構成する

1. *ConsoleApplication* フォルダーで、`appsettings.json.example` ファイルの名前を `appsettings.json` に変更します
1. `appsettings.json` ファイルを開いて編集し、次の変更を加えます
    1. `ClientId` が `YOUR_CLIENT_ID_HERE` として設定されている行を見つけ、既存の値を Azure ポータルからコピーされた `ConsoleApp-DeltaQuery-DotNet` アプリケーションのアプリケーション ID (clientId) に置き換えます。

ソリューションをクリーンアップし、ソリューションを再構築して、デバッガーで開始します。

## 投稿

このサンプルに投稿する場合は、[CONTRIBUTING.MD](/CONTRIBUTING.md) を参照してください。

このプロジェクトでは、[Microsoft Open Source Code of Conduct (Microsoft オープン ソース倫理規定)](https://opensource.microsoft.com/codeofconduct/) が採用されています。詳細については、「[Code of Conduct の FAQ (倫理規定の FAQ)](https://opensource.microsoft.com/codeofconduct/faq/)」を参照してください。また、その他の質問やコメントがあれば、[opencode@microsoft.com](mailto:opencode@microsoft.com) までお問い合わせください。

## 質問とコメント

WebJobs SDK を使用して、Microsoft Graph Webhook のサンプルに関するフィードバックをぜひお寄せください。質問や提案は、このリポジトリの「[問題](https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet/issues)」セクションで送信できます。

Microsoft Graph 全般の質問については、「[Stack Overflow](https://stackoverflow.com/questions/tagged/MicrosoftGraph)」に投稿してください。質問やコメントには、必ず "*MicrosoftGraph*" とタグを付けてください。

機能に関して提案がございましたら、「[User Voice](https://officespdev.uservoice.com/)」ページでアイデアを投稿してから、その提案に投票してください。

## その他の技術情報

- [AAD DQ サンプル](https://github.com/Azure-Samples/active-directory-dotnet-graphapi-diffquery)
- [Working with Delta Query in Microsoft Graph (Microsoft Graph でのデルタ クエリの操作)](https://developer.microsoft.com/en-us/graph/docs/concepts/delta_query_overview)
- [Microsoft Graph 開発者向けサイト](https://developer.microsoft.com/en-us/graph/)
- [ASP.NET MVC アプリで Microsoft Graph を呼び出す](https://developer.microsoft.com/en-us/graph/docs/platform/aspnetmvc)
- [MSAL.NET](https://aka.ms/msal-net)

Copyright (c) 2019 Microsoft Corporation.All rights reserved.
