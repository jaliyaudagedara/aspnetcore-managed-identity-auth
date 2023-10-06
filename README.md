# More read:
[Calling an ASP.NET Core Web API Secured with Microsoft Entra ID using Azure Managed Identity](https://jaliyaudagedara.blogspot.com/2023/10/calling-aspnet-core-web-api-secured.html)

## Assign the managed identity access to the app role.

```powershell
# Login to Azure and setting the subscription
Connect-AzAccount
Set-AzContext -SubscriptionId "<SubscriptionId>"

# Install Microsoft.Graph Module if required using below command
# Install-Module Microsoft.Graph

# Invoking Connect-MgGraph before any commands that access Microsoft Graph,
# Requesting scopes that we require during our session
$tenantID = '<TenantId>'
Connect-MgGraph -TenantId $tenantId -Scopes 'Application.Read.All', 'Application.ReadWrite.All', 'AppRoleAssignment.ReadWrite.All', 'Directory.AccessAsUser.All', 'Directory.Read.All', 'Directory.ReadWrite.All'
 
# App Registration Name
$appRegistrationName = 'Managed Identity Auth Demo'

# Retrieving Service Principal Id.
$servicePrincipal = (Get-MgServicePrincipal -Filter "DisplayName eq '$appRegistrationName'")
$servicePrincipalObjectId = $servicePrincipal.Id

# Retrieving App role Id that the Managed Identity should be assigned to
$appRoleName = 'MI.Access'
$appRoleId = ($servicePrincipal.AppRoles | Where-Object {$_.Value -eq $appRoleName }).Id
 
# Managed Identity's Object (principal) ID.
$managedIdentityObjectId = '<ManagedIdentityObjectId>'
 
# Assign the managed identity access to the app role.
New-MgServicePrincipalAppRoleAssignment `
    -ServicePrincipalId $servicePrincipalObjectId `
    -PrincipalId $managedIdentityObjectId `
    -ResourceId $servicePrincipalObjectId `
    -AppRoleId $appRoleId
```