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

Este aplicativo de console demonstra como fazer chamadas de consulta Delta à API do Graph, permitindo que aplicativos solicitem somente os dados alterados de locatários do Microsoft Graph.

O aplicativo de exemplo demonstra como chamadas do Graph podem ser feitas com o SDK do Graph e como as respostas podem ser manejas.

O exemplo específico usado neste aplicativo de exemplo envolve monitoramento de alterações (adição e remoção) de MailFolders em uma conta de email individual.

## Como executar esse aplicativo de exemplo

Para executar este exemplo, você precisará do seguinte:
-Visual Studio 2017
- Uma conexão com a Internet
– Um locatário do Azure Active Directory (Azure AD). Para obter mais informações sobre como obter um locatário do Azure AD, confira [Como obter um locatário do Azure AD](https://azure.microsoft.com/en-us/documentation/articles/active-directory-howto-tenant/). 

### Etapa 1: Clone ou baixe este repositório

A partir de seu shell ou linha de comando:

`git clone https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet`

### Etapa 2: Registre seu aplicativo de exemplo com o locatário do Azure Active Directory

Há um projeto neste exemplo. Para registrá-lo, você pode:

- ou seguir as instruções das etapas [Etapa 2: Registre seu aplicativo de exemplo com o locatário do Azure Active Directory](#step-2-register-the-sample-with-your-azure-active-directory-tenant) e[Etapa 3: Configure seu aplicativo de exemplo para usar seu locatário Azure AD ](#choose-the-azure-ad-tenant-where-you-want-to-create-your-applications)
- ou use scripts do PowerShell que:
  - **automaticamente ** criam os aplicativos do Azure AD e objetos relacionados (senhas, permissões e dependências) para você
  - modifiquem os arquivos de configuração dos projetos do Visual Studio.

Se você quiser usar essa automação:

1. No Windows, execute o PowerShell e navegue até a raiz do diretório clonado
1. No PowerShell, execute:

   ```PowerShell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force
   ```

1. Execute o script para criar seu aplicativo Azure AD e configure o código do aplicativo de exemplo adequadamente.

   ```PowerShell
   .\AppCreationScripts\Configure.ps1
   ```

   > Outras maneiras de executar os scripts estão descritas na [Scripts de criação de aplicativos](./AppCreationScripts/AppCreationScripts.md)

1. Abra a solução do Visual Studio e clique em iniciar

Se não quiser usar essa automação, siga as etapas abaixo.

#### Escolha o locatário do Azure AD no local em que você deseja criar seus aplicativos

Como primeira etapa, você precisará:

1. Entre no [portal do Azure](https://portal.azure.com)usando uma conta corporativa, de estudante ou uma conta Microsoft pessoal.
1. Se sua conta permitir o acesso a mais de um locatário, clique na sua conta no canto superior direito e configure sua sessão do portal ao locatário do Azure AD desejado (usando **Mudar Diretório**).
1. No painel de navegação à esquerda, selecione o serviço **Azure Active Directory**e, em seguida, selecione **Registros de aplicativo (Visualização)**.

#### Registre o aplicativo cliente (ConsoleApp-DeltaQuery-DotNet)

1. Na página **Registros de aplicativo (Visualização)**, selecione **Registrar um aplicativo**.
1. Quando a página **Registrar um aplicativo** for exibida, insira as informações de registro do aplicativo:
   - Na seção **Nome**, insira um nome de aplicativo relevante que será exibido aos usuários do aplicativo, por exemplo, `ConsoleApp-DeltaQuery-DotNet`.
   - Na seção **Tipos de conta com suporte**, selecione **Contas em qualquer diretório organizacional e contas pessoais do Microsoft (por exemplo: Skype, Xbox, Outlook.com)**.
   - Selecione **Registrar** para criar o aplicativo.
1. Na página **Visão geral** do aplicativo, encontre o valor de **ID do aplicativo (cliente)** e registre-o para usar mais tarde. Será necessário configurar o arquivo de configuração do Visual Studio para este projeto.
1. Na lista de páginas do aplicativo, selecione **Autenticação**
   - Em *URIs de redirecionamento sugeridas para clientes públicos(celular,computador)*, marque a segunda caixa para que o aplicativo possa trabalhar com a biblioteca do MSAL usada no aplicativo. (A caixa dever conter a opção *urn:ietf:wg:oauth:2.0:oob*). 
1. Na lista de páginas do aplicativo, selecione **Permissões de API**.
   - Clique no botão **Adicionar uma permissão** e, em seguida,
   - Certifique-se de que a guia **APIs da Microsoft** esteja selecionada
   - Na seção *APIs mais usadas da Microsoft*, clique em **Microsoft Graph**
   - Na seção **Permissões delegadas**, verifique se as permissões corretas estão marcadas: **Mail.Read**. Use a caixa de pesquisa, se necessário.
   - Selecione o botão **Adicionar permissões**

### Etapa 3: Configurar o exemplo para usar seu locatário do Azure AD

Nas etapas abaixo, "ClientID" é o mesmo que " ID do aplicativo" or "ID do app".

Abra o recurso solução no Visual Studio para configurar os projetos.

#### Configure o projeto cliente 

1. Na pasta *ConsoleApplication*, renomeie o arquivo `appsettings.json.example` para `appsettings.json`
1. Abra e edite o arquivo `appsettings.json` para fazer a seguinte alteração
    1. Localize a linha onde `ClientId` está definida como `YOUR_CLIENT_ID_HERE` e substitua o valor existente pelo ID do aplicativo (clientId) do aplicativo `ConsoleApp-DeltaQuery-DotNet` copiado do portal do Azure.

Limpe a solução, recrie a solução e inicie-a no depurador.

## Colaboração

Se quiser contribuir para esse exemplo, confira [CONTRIBUTING.MD](/CONTRIBUTING.md).

Este projeto adotou o [Código de Conduta de Código Aberto da Microsoft](https://opensource.microsoft.com/codeofconduct/).  Para saber mais, confira as [Perguntas frequentes sobre o Código de Conduta](https://opensource.microsoft.com/codeofconduct/faq/) ou entre em contato pelo [opencode@microsoft.com](mailto:opencode@microsoft.com) se tiver outras dúvidas ou comentários.

## Perguntas e comentários

Gostaríamos de receber seus comentários sobre o exemplo Microsoft Graph Webhooks usando o WebJobs SDK. Você pode nos enviar perguntas e sugestões na seção [Problemas](https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet/issues) deste repositório.

Em geral, as perguntas sobre o Microsoft Graph devem ser postadas no [Stack Overflow](https://stackoverflow.com/questions/tagged/MicrosoftGraph). Verifique se suas perguntas ou comentários estão marcados com *\[MicrosoftGraph]*.

Se você tiver uma sugestão de recurso, poste sua ideia na nossa página em [Voz do Usuário](https://officespdev.uservoice.com/) e vote em suas sugestões.

## Recursos adicionais

- [Exemplo de Consulta Delta (DQ) no AAD](https://github.com/Azure-Samples/active-directory-dotnet-graphapi-diffquery)
- [Trabalhando com Consulta Delta no Microsoft Graph](https://developer.microsoft.com/en-us/graph/docs/concepts/delta_query_overview)
- [Site do desenvolvedor do Microsoft Graph](https://developer.microsoft.com/en-us/graph/)
- [Chamar o Microsoft Graph em um aplicativo do ASP.NET MVC](https://developer.microsoft.com/en-us/graph/docs/platform/aspnetmvc)
- [MSAL.NET](https://aka.ms/msal-net)

Direitos autorais (c) 2019 Microsoft Corporation. Todos os direitos reservados.
