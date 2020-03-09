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

Cette application de console montre comment effectuer des appels de requête delta à l’API Graph pour permettre aux applications de demander uniquement les données modifiées aux clients Microsoft Graph.

L’exemple montre comment les appels de graphiques peuvent être effectués avec le kit de développement logiciel (SDK) Graph et comment les réponses peuvent être gérées.

L’exemple spécifique utilisé dans cet exemple implique la surveillance des modifications (ajout et suppression) de MailFolders dans le compte de messagerie d’une personne.

## Exécution de cet exemple

Pour exécuter cet exemple, vous avez besoin des éléments suivants :
- Visual Studio 2017
- une connexion Internet
- un client Azure Active Directory (Azure AD). Pour plus d’informations sur la façon d’obtenir un client Azure AD, voir [Obtention d’un client Azure AD](https://azure.microsoft.com/en-us/documentation/articles/active-directory-howto-tenant/) 

### Étape 1 : Clonage ou téléchargement de ce référentiel

À partir de votre shell ou de la ligne de commande :

`git clone https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet`

### Étape 2 : Inscription de l’exemple d’application avec votre client Azure Active Directory

Cet exemple contient un seul projet. Pour l’enregistrer, vous pouvez :

- soit suivre la procédure [Étape 2 : Inscription de l’exemple d’application avec votre client Azure Active Directory](#step-2-register-the-sample-with-your-azure-active-directory-tenant) et [Étape 3 : Configuration de l’échantillon pour utiliser votre client Azure AD](#choose-the-azure-ad-tenant-where-you-want-to-create-your-applications)
- soit utiliser des scripts PowerShell qui :
  - créent **automatiquement** les applications Azure AD et les objets associés (mots de passe, autorisations, dépendances) à votre place
  - modifient les fichiers de configuration des projets Visual Studio.

Si vous voulez utiliser cette automatisation :

1. Sous Windows, exécutez PowerShell et accédez à la racine du répertoire cloné
1. À partir de PowerShell, exécutez :

   ```PowerShell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force
   ```

1. Exécutez le script pour créer votre application Azure AD et configurez le code de l’exemple d’application en conséquence.

   ```PowerShell
   .\AppCreationScripts\Configure.ps1
   ```

   > D’autres méthodes d’exécution des scripts sont décrites dans [Scripts de création d’applications](./AppCreationScripts/AppCreationScripts.md)

1. Ouvrez le fichier de solution Visual Studio et cliquez sur Démarrer.

Si vous ne voulez pas utiliser cette automatisation, suivez les étapes ci-dessous.

#### Choix du client Azure AD où créer vos applications

Pour commencer, vous devez effectuer les opérations suivantes :

1. Connectez-vous au [portail Microsoft Azure](https://portal.azure.com) à l’aide d’un compte professionnel ou scolaire, ou d’un compte Microsoft personnel.
1. Si votre compte vous propose un accès à plusieurs clients, sélectionnez votre compte en haut à droite et définissez votre session de portail sur le client Azure AD souhaité (à l’aide de **Changer de répertoire**).
1. Dans le volet de navigation gauche, sélectionnez le service **Azure Active Directory**, puis sélectionnez **Inscriptions d’applications (préversion)**.

#### Inscription de l’application cliente (ConsoleApp-DeltaQuery-DotNet)

1. À la page **Inscriptions des applications (préversion)**, sélectionnez **Inscrire une application**.
1. Lorsque la **page Inscrire une application** s’affiche, saisissez les informations d’inscription de votre application :
   - Dans la section **Nom**, saisissez un nom d’application significatif qui s’affichera pour les utilisateurs de l’application, par exemple `ConsoleApp-DeltaQuery-DotNet`.
   - Dans la section **Types de comptes pris en charge**, sélectionnez **Comptes dans un annuaire organisationnel et comptes personnels Microsoft (par ex. Skype, Xbox, Outlook.com)**.
   - Sélectionnez **S’inscrire** pour créer l’application.
1. Sur la page **Vue d’ensemble** de l’application, notez la valeur **ID d’application (client)** et conservez-la pour plus tard. Vous en aurez besoin pour paramétrer le fichier de configuration de Visual Studio pour ce projet.
1. Dans la liste des pages de l’application, sélectionnez **Authentification**.
   - Dans la boîte de dialogue *URI de redirection suggérés pour les clients publics (mobile, bureau)*, cochez la deuxième zone pour que l’application puisse utiliser les bibliothèques MSAL utilisées dans l’application. (La zone doit contenir l’option *urn:ietf:wg:oauth:2.0:oob*). 
1. Dans la liste des pages de l’application, sélectionnez **Permissions API**.
   - Cliquez sur le bouton **Ajouter une autorisation** puis,
   - Vérifiez que l'onglet **API Microsoft** est sélectionné
   - Dans la section *API Microsoft couramment utilisées*, cliquez sur **Microsoft Graph**
   - Dans la section **Autorisations déléguées**, assurez-vous que les autorisations appropriées sont vérifiées : **Mail.Read**. Utilisez la zone de recherche, le cas échéant.
   - Cliquez sur le bouton **Ajouter des autorisations**

### Étape 3 : Configuration de l’échantillon pour utiliser votre client Azure AD

Dans les étapes ci-dessous, « ID client » est identique à « ID de l’application ».

Ouverture de la solution dans Visual Studio pour configurer les projets

#### Configurer le projet client

1. Dans le dossier *ConsoleApplication*, renommez le fichier `appsettings.json.example` en `appsettings.json`.
1. Ouvrez et modifiez le fichier `appSettings.json` pour apporter la modification suivante :
    1. Recherchez la ligne dans laquelle `ClientId` est défini comme `YOUR_CLIENT_ID_HERE` et remplacez la valeur existante par l’ID (client) de l’application `ConsoleApp-DeltaQuery-DotNet` copié à partir du portail Azure.

Nettoyez la solution, reconstruisez-la, puis démarrez-la dans le débogueur.

## Contribution

Si vous souhaitez contribuer à cet exemple, voir [CONTRIBUTING.MD](/CONTRIBUTING.md).

Ce projet a adopté le [code de conduite Open Source de Microsoft](https://opensource.microsoft.com/codeofconduct/). Pour en savoir plus, reportez-vous à la [FAQ relative au code de conduite](https://opensource.microsoft.com/codeofconduct/faq/) ou contactez [opencode@microsoft.com](mailto:opencode@microsoft.com) pour toute question ou tout commentaire.

## Questions et commentaires

N’hésitez pas à nous faire part de vos commentaires sur l’exemple de webhooks Microsoft Graph utilisant le kit de développement logiciel webjobs. Vous pouvez nous faire part de vos questions et suggestions dans la rubrique [Problèmes](https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet/issues) de ce référentiel.

Les questions générales sur Microsoft Graph doivent être publiées sur le [Dépassement de capacité de la pile](https://stackoverflow.com/questions/tagged/MicrosoftGraph). Veillez à poser vos questions ou à rédiger vos commentaires en utilisant les tags *\[MicrosoftGraph]*.

Si vous avez des suggestions de fonctionnalité, soumettez votre idée sur notre page [Voix utilisateur](https://officespdev.uservoice.com/) et votez pour votre suggestion.

## Ressources supplémentaires

- [Exemple AAD DQ](https://github.com/Azure-Samples/active-directory-dotnet-graphapi-diffquery)
- [Utilisation de la requête delta dans Microsoft Graph](https://developer.microsoft.com/en-us/graph/docs/concepts/delta_query_overview)
- [Site des développeurs de Microsoft Graph](https://developer.microsoft.com/en-us/graph/)
- [Appel de Microsoft Graph dans une application ASP.NET MVC](https://developer.microsoft.com/en-us/graph/docs/platform/aspnetmvc)
- [MSAL.NET](https://aka.ms/msal-net)

Copyright (c) 2019 Microsoft Corporation. Tous droits réservés.
