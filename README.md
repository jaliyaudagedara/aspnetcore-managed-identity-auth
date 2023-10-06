# Login to Azure and setting the subscription
Connect-AzAccount
Set-AzContext -SubscriptionId "4e7d5747-5253-4cb5-ab61-db99bffdc924"

# Install Microsoft.Graph Module if required using below command
# Install-Module Microsoft.Graph

# Invoking Connect-MgGraph before any commands that access Microsoft Graph,
# Requesting scopes that we require during our session
$tenantID = '1f4a5f26-b0bc-402c-9347-e0f7d16c098f'
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
$managedIdentityObjectId = 'c774b12d-6c77-496b-ba4d-cd08f7444bc4'
 
# Assign the managed identity access to the app role.
New-MgServicePrincipalAppRoleAssignment `
    -ServicePrincipalId $servicePrincipalObjectId `
    -PrincipalId $managedIdentityObjectId `
    -ResourceId $servicePrincipalObjectId `
    -AppRoleId $appRoleId