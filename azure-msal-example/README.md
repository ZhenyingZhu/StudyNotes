# Azure MSAL Client-Server Example

This example demonstrates how to build a secure client-server application using Microsoft Authentication Library (MSAL) with Azure Active Directory. The server is an ASP.NET Core Web API that requires authentication, and the client is a .NET console application that authenticates users to make API calls.

## Architecture

- **Server**: ASP.NET Core Web API with JWT Bearer authentication using Microsoft.Identity.Web
- **Client**: .NET Console application using Microsoft.Identity.Client (MSAL) for authentication
- **Authentication**: Azure Active Directory with OAuth 2.0 / OpenID Connect

## Prerequisites

1. Azure subscription with Azure Active Directory
2. .NET 8.0 SDK
3. Visual Studio 2022 or VS Code (optional)

## Azure AD Setup

### Step 1: Register the Server Application

1. Navigate to [Azure Portal](https://portal.azure.com) → Azure Active Directory → App registrations
2. Click **New registration**
3. Fill in the details:
   - **Name**: `MSAL-Server-API`
   - **Supported account types**: Accounts in this organizational directory only
   - **Redirect URI**: Leave empty for now
4. Click **Register**
5. Note down the **Application (client) ID** and **Directory (tenant) ID**

### Step 2: Configure the Server Application

1. In the server app registration, go to **Expose an API**
2. Click **Add a scope**
3. For Application ID URI, click **Save and continue** (it will use the default)
4. Add a scope:
   - **Scope name**: `Message.Read`
   - **Who can consent**: Admins and users
   - **Admin consent display name**: `Read messages`
   - **Admin consent description**: `Allow the application to read messages`
   - **User consent display name**: `Read messages`
   - **User consent description**: `Allow the application to read messages`
   - **State**: Enabled
5. Click **Add scope**

### Step 3: Register the Client Application

1. Create another app registration:
   - **Name**: `MSAL-Client-Console`
   - **Supported account types**: Accounts in this organizational directory only
   - **Redirect URI**: Public client/native → `http://localhost`
2. Click **Register**
3. Note down the **Application (client) ID**

### Step 4: Configure the Client Application

1. In the client app registration, go to **Authentication**
2. Under **Advanced settings**, set **Allow public client flows** to **Yes**
3. Go to **API permissions**
4. Click **Add a permission** → **My APIs**
5. Select your server application (`MSAL-Server-API`)
6. Select **Delegated permissions**
7. Check `Message.Read` scope
8. Click **Add permissions**
9. Click **Grant admin consent** (if you have admin rights)

## Configuration

### Server Configuration

Update `Server/appsettings.json` and `Server/appsettings.Development.json`:

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "your-tenant-domain.onmicrosoft.com",
    "TenantId": "your-tenant-id",
    "ClientId": "your-server-app-client-id",
    "CallbackPath": "/signin-oidc",
    "Audience": "api://your-server-app-client-id"
  }
}
```

Replace:
- `your-tenant-domain.onmicrosoft.com`: Your Azure AD tenant domain
- `your-tenant-id`: Directory (tenant) ID from server app registration
- `your-server-app-client-id`: Application (client) ID from server app registration

### Client Configuration

Update `Client/appsettings.json`:

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "your-tenant-id",
    "ClientId": "your-client-app-client-id",
    "RedirectUri": "http://localhost"
  },
  "Api": {
    "BaseUrl": "https://localhost:7001",
    "Scopes": [ "api://your-server-app-client-id/Message.Read" ]
  }
}
```

Replace:
- `your-tenant-id`: Directory (tenant) ID (same as server)
- `your-client-app-client-id`: Application (client) ID from client app registration
- `your-server-app-client-id`: Application (client) ID from server app registration (used in scope)

## Running the Application

### 1. Start the Server

```bash
cd Server
dotnet restore
dotnet run
```

The server will start at `https://localhost:7001` and `http://localhost:5000`. You can visit `https://localhost:7001/swagger` to see the API documentation.

### 2. Start the Client

In a new terminal:

```bash
cd Client
dotnet restore
dotnet run
```

### 3. Test the Application

1. The client will prompt you to authenticate via a web browser
2. Sign in with your Azure AD account
3. After successful authentication, you'll see a menu with options:
   - **Send a message to the server**: Enter a message that will be echoed back
   - **Get protected data**: Retrieve protected information from the server
   - **Exit**: Close the application

## API Endpoints

### POST /api/Message/echo
- **Description**: Echoes back the message sent by the client
- **Authentication**: Required (Bearer token)
- **Scope**: `Message.Read`
- **Request Body**:
  ```json
  {
    "message": "Your message here"
  }
  ```
- **Response**:
  ```json
  {
    "originalMessage": "Your message here",
    "echoMessage": "Echo: Your message here",
    "receivedAt": "2024-01-01T12:00:00.000Z",
    "userName": "user@domain.com"
  }
  ```

### GET /api/Message/protected
- **Description**: Returns protected data accessible only to authenticated users
- **Authentication**: Required (Bearer token)
- **Scope**: `Message.Read`
- **Response**:
  ```json
  {
    "message": "This is protected data",
    "user": "user@domain.com",
    "timestamp": "2024-01-01T12:00:00.000Z"
  }
  ```

## Key Features

### Server Features
- **JWT Bearer Authentication**: Uses Microsoft.Identity.Web for seamless Azure AD integration
- **Scope-based Authorization**: Validates required scopes in access tokens
- **Claims Processing**: Extracts user information from JWT claims
- **CORS Support**: Configured for development scenarios
- **Swagger Integration**: API documentation and testing interface

### Client Features
- **Interactive Authentication**: Opens browser for user sign-in
- **Silent Token Refresh**: Automatically refreshes tokens when needed
- **Token Caching**: Stores tokens securely for better user experience
- **Error Handling**: Comprehensive error handling for authentication and API calls
- **Menu-driven Interface**: Easy-to-use console interface

## Security Considerations

1. **Token Storage**: Tokens are cached securely by MSAL
2. **HTTPS**: All communication should use HTTPS in production
3. **Scope Validation**: Server validates required scopes for each endpoint
4. **Token Expiration**: Tokens are automatically refreshed when expired
5. **Confidential vs Public Clients**: Server uses confidential client pattern, console uses public client

## Troubleshooting

### Common Issues

1. **Authentication Failed**
   - Verify Azure AD app registrations are correct
   - Check that API permissions are granted
   - Ensure redirect URIs match configuration

2. **API Calls Return 401 Unauthorized**
   - Verify the scope in client configuration matches server expectations
   - Check that admin consent was granted for API permissions
   - Ensure server configuration has correct tenant and client IDs

3. **Token Acquisition Errors**
   - Clear token cache: Delete files in `%USERPROFILE%\.msalcache` (Windows)
   - Verify tenant ID and client IDs are correct
   - Check network connectivity and firewall settings

4. **CORS Issues**
   - Server includes CORS policy for development
   - For production, configure specific origins instead of allowing all

### Logging

Both applications include detailed logging:
- **Server**: Check console output for authentication and authorization logs
- **Client**: MSAL provides detailed authentication flow information

## Production Deployment

For production deployment:

1. **Server**:
   - Use Azure App Service or Azure Container Instances
   - Configure proper CORS policies
   - Use Azure Key Vault for sensitive configuration
   - Enable Application Insights for monitoring

2. **Client**:
   - Consider using a web application instead of console for better user experience
   - Implement proper error handling and user feedback
   - Use secure token storage mechanisms

## Additional Resources

- [Microsoft Identity Platform Documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/)
- [MSAL.NET Documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-overview)
- [Microsoft.Identity.Web Documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/microsoft-identity-web)
- [Azure AD App Registration Documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app)