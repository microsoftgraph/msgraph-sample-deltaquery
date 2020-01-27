[CmdletBinding()]
param(
    [PSCredential] $Credential,
    [Parameter(Mandatory=$False, HelpMessage='Tenant ID (This is a GUID which represents the "Directory ID" of the AzureAD tenant into which you want to create the apps')]
    [string] $tenantId
)

<#
 This script creates the Azure AD application needed for this sample.

 Before running this script you need to install the AzureAD cmdlets as an administrator.
 For this:
 1) Run Powershell as an administrator
 2) In the PowerShell window, type: Install-Module AzureAD
#>

# If user passed credentials, use those
# Otherwise, prompt the user to sign in
if ($Credential)
{
    $session = Connect-AzureAD -Credential $Credential
}
else
{
    $session = Connect-AzureAD
}

# Get the user's object from Azure AD, we need the object ID
$user = Get-AzureADUser -ObjectId $session.Account.Id

Write-Host "Creating app registration..." -NoNewline

# Create the application
# Reply url: This is the standard reply URL used by MSAL with the device code flow
# Available to other tenants: Set to true here for simplification (don't need to specify tenant ID)
# Public client: Necessary to enable the device code flow
$app = New-AzureADApplication -DisplayName "Delta Query Console Sample" `
                              -ReplyUrls "https://login.microsoftonline.com/common/oauth2/nativeclient" `
                              -AvailableToOtherTenants $true `
                              -PublicClient $true

Write-Host "DONE" -ForeGroundColor Green

Write-Host "Creating service principal..." -NoNewline

# Create the app's service principal
$servicePrincipal = New-AzureADServicePrincipal -AppId $app.AppId -Tags {WindowsAzureActiveDirectoryIntegratedApp}

Write-Host "DONE" -ForeGroundColor Green

# If the user was not added as an owner, add them now
$owner = Get-AzureADApplicationOwner -ObjectId $app.ObjectId
if ($null -eq $owner)
{
    Write-Host "Adding $($user.DisplayName) (object ID $($user.ObjectId)) as owner..." -NoNewline

    # Add the user as the owner of the application
    # This conforms with the behavior in the Azure portal
    Add-AzureADApplicationOwner -ObjectId $app.ObjectId -RefObjectId $user.ObjectId

    Write-Host "DONE" -ForeGroundColor Green
}

Write-Host ""
Write-Host "App creation successful. Your app ID is: " -NoNewline
Write-Host $($app.AppId) -ForegroundColor Cyan

Disconnect-AzureAD