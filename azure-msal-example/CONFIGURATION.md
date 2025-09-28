# Azure MSAL Example Configuration Template

This file contains the configuration values you need to update after setting up your Azure AD applications.

## Server Configuration (Server/appsettings.json)

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "[YOUR_TENANT_DOMAIN].onmicrosoft.com",
    "TenantId": "[YOUR_TENANT_ID]",
    "ClientId": "[YOUR_SERVER_APP_CLIENT_ID]",
    "CallbackPath": "/signin-oidc",
    "Audience": "api://[YOUR_SERVER_APP_CLIENT_ID]"
  }
}
```

## Client Configuration (Client/appsettings.json)

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "[YOUR_TENANT_ID]",
    "ClientId": "[YOUR_CLIENT_APP_CLIENT_ID]",
    "RedirectUri": "http://localhost"
  },
  "Api": {
    "BaseUrl": "https://localhost:7001",
    "Scopes": [ "api://[YOUR_SERVER_APP_CLIENT_ID]/Message.Read" ]
  }
}
```

## Values to Replace

- `[YOUR_TENANT_DOMAIN]`: Your Azure AD tenant domain (e.g., "contoso")
- `[YOUR_TENANT_ID]`: Directory (tenant) ID from Azure portal
- `[YOUR_SERVER_APP_CLIENT_ID]`: Application (client) ID of the server app registration
- `[YOUR_CLIENT_APP_CLIENT_ID]`: Application (client) ID of the client app registration

## Quick Setup Checklist

1. [ ] Created server app registration in Azure AD
2. [ ] Added "Message.Read" scope to server app
3. [ ] Created client app registration in Azure AD
4. [ ] Enabled "Allow public client flows" for client app
5. [ ] Added API permission to client app for server app's scope
6. [ ] Granted admin consent for API permissions
7. [ ] Updated server appsettings.json with correct values
8. [ ] Updated client appsettings.json with correct values
9. [ ] Built and tested both applications