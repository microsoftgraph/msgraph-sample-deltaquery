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

Esta aplicación de consola demuestra cómo realizar llamadas de Consulta Delta a la Graph API, permitiendo que las aplicaciones soliciten sólo datos modificados a los inquilinos de Microsoft Graph.

El ejemplo utiliza demostraciones de cómo se pueden hacer llamadas de Graph con el SDK de Graph y cómo se pueden manejar las respuestas.

El ejemplo específico usado en este ejemplo implica la supervisión de los cambios (suma y eliminación) de MailFolders en la cuenta de correo electrónico de un usuario.

## Como ejecutar este ejemplo

Para ejecutar este ejemplo necesitará:
- Visual Studio 2017
- Una conexión a Internet
- Un inquilino del Azure Active Directory (Azure AD). Para más información sobre cómo conseguir un inquilino de Azure AD, por favor consulte [cómo conseguir un inquilino de Azure AD](https://azure.microsoft.com/en-us/documentation/articles/active-directory-howto-tenant/)  

### Paso 1: Clone o descargue este repositorio

Desde la línea de comandos o shell:

`clonación de git https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet`

### Paso 2: Registre la solicitud de muestra con su inquilino del Azure Active Directory

Hay un proyecto en este ejemplo. Para registrarlo, puede:

- o bien siga los pasos [paso 2: Registre la muestra con su inquilino del Azure Active Directory ](#step-2-register-the-sample-with-your-azure-active-directory-tenant)y[ paso 3: Configurar el ejemplo para usar el inquilino de Azure AD](#choose-the-azure-ad-tenant-where-you-want-to-create-your-applications)
- o use los scripts de PowerShell que:
  - **automáticamente** crea las aplicaciones de Azure AD y los objetos relacionados (contraseñas, permisos, dependencias) para ti
  - modifica los archivos de configuración de los proyectos de Visual Studio.

Si desea usar esta automatización:

1. En Windows ejecute PowerShell y navegue hasta la raíz del directorio clonado
1. En PowerShell, ejecute:

   ```PowerShell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force
   ```

1. Ejecute el script para crear su aplicación Azure AD y configure el código de la aplicación de muestra de acuerdo con ello.

   ```PowerShell
   .\AppCreationScripts\Configure.ps1
   ```

   > Otras formas de ejecutar los guiones se describen en [scripts de creación de aplicaciones ](./AppCreationScripts/AppCreationScripts.md)

1. Abra la solución de Visual Studio y haga clic en inicio.

Si no quieres usar esta automatización, sigue los siguientes pasos a continuación

#### Elija el inquilino de Azure AD donde quiere crear sus aplicaciones

Como primer paso, tendrá que:

1. Iniciar sesión en el[ Azure Portal](https://portal.azure.com)usando una cuenta de trabajo o de escuela, o una cuenta personal de Microsoft.
1. Si su cuenta le da acceso a más de un inquilino, seleccione su cuenta en la esquina superior derecha y establezca su sesión de portal con el inquilino Azure AD deseado (usando**cambiar directorio**).
1. En el panel de navegación de la izquierda, seleccione el servicio de**Azure Active Directory**, y luego seleccione **registros de aplicaciones (vista previa)**.

#### Registrar la aplicación cliente (ConsoleApp-DeltaQuery-DotNet)

1. En la página **Registros de aplicaciones (versión preliminar)**, seleccione **Registrar una aplicación**.
1. Cuando aparezca la **página registrar una aplicación**, escriba la información de registro de su aplicación:
   - En la sección **Nombre**, introducir un nombre de aplicación significativo que se mostrará a los usuarios de la aplicación, por ejemplo`ConsoleApp-DeltaQuery-DotNet`.
   - En la sección **tipos de cuentas admitidas**, seleccione **cuentas en cualquier directorio organizacional y cuentas personales de Microsoft (por ejemplo, Skype, Xbox, Outlook.com)**.
   - Seleccione **registrar** para crear la aplicación.
1. En la página **Información general** de la aplicación, busque el valor **Id. de la aplicación (cliente)** y guárdelo para más tarde. Lo necesitarás para configurar el archivo de configuración de Visual Studio para este proyecto.
1. En la lista de páginas de la aplicación, seleccione **autenticación**.
   - En los *URI de redirección sugeridos para clientes públicos (móvil, escritorio)*, marque el segundo cuadro para que la aplicación pueda funcionar con las bibliotecas de MSAL utilizadas en la aplicación. (El cuadro debe contener la opción*urn:ietf:wg:oauth:2.0:oob*). 
1. En la lista de páginas de la aplicación, seleccione **Permisos de API**.
   - Haga clic en el botón **Agregar un permiso**.
   - Asegúrese de que la pestaña **API de Microsoft** está seleccionada.
   - En la sección *API de Microsoft más usadas*, haga clic en **Microsoft Graph**.
   - En la sección **Permisos delegados**, asegúrese de que se comprueben los permisos correctos: **Mail.Read**. Si es necesario, use el cuadro de búsqueda.
   - Seleccione el botón **Agregar permisos**.

### Paso 3: Configure el ejemplo para usar el inquilino de Azure AD

En los pasos siguientes, "ClientID" es lo mismo que "Application ID" o "AppId".

Abra la solución en Visual Studio para configurar los proyectos

#### Configure el proyecto del cliente

1. En la carpeta *ConsoleApplication*, renombre el archivo `appsettings.json.example` a `appsettings.json`
1. Abra y edite el archivo `appsettings.json` para hacer el siguiente cambio
    1. Encuentre la línea donde se establece`ClientId` como `YOUR_CLIENT_ID_HERE` y reemplace el valor existente con el ID de la aplicación (clientId) de la aplicación `ConsoleApp-DeltaQuery-DotNet` copiada del Azure portal.

Limpie la solución, reconstruya la solución e inicie en el depurador.

## Colaboradores

Si quiere hacer su aportación a este ejemplo, vea [CONTRIBUTING.MD](/CONTRIBUTING.md).

Este proyecto ha adoptado el [Código de conducta de código abierto de Microsoft](https://opensource.microsoft.com/codeofconduct/). Para obtener más información, vea [Preguntas frecuentes sobre el código de conducta](https://opensource.microsoft.com/codeofconduct/faq/) o póngase en contacto con [opencode@microsoft.com](mailto:opencode@microsoft.com) si tiene otras preguntas o comentarios.

## Preguntas y comentarios

Nos encantaría recibir sus comentarios sobre Webhooks de Microsoft Graph con el SDK de WebJobs. Puede enviarnos sus preguntas y sugerencias a través de la sección [Problemas](https://github.com/microsoftgraph/ConsoleApp-DeltaQuery-DotNet/issues) de este repositorio.

Las preguntas sobre Microsoft Graph en general deben publicarse en [desbordamiento de pila](https://stackoverflow.com/questions/tagged/MicrosoftGraph). Asegúrese de que sus preguntas o comentarios estén etiquetados con *\[MicrosoftGraph]*.

Si quiere sugerir alguna función, publique su idea en nuestra página de [User Voice](https://officespdev.uservoice.com/) y vote por sus sugerencias.

## Recursos adicionales

- [Ejemplo AAD DQ](https://github.com/Azure-Samples/active-directory-dotnet-graphapi-diffquery)
- [Trabajando con Delta Query en Microsoft Graph ](https://developer.microsoft.com/en-us/graph/docs/concepts/delta_query_overview)
- [Sitio para desarrolladores de Microsoft Graph](https://developer.microsoft.com/en-us/graph/)
- [Llamar a Microsoft Graph desde una aplicación de ASP.NET MVC](https://developer.microsoft.com/en-us/graph/docs/platform/aspnetmvc)
- [MSAL.NET](https://aka.ms/msal-net)

Copyright (c) 2019 Microsoft Corporation. Todos los derechos reservados.
