## Azure

### Portal
https://portal.azure.com/

https://manage.windowsazure.com

### Active directory
name service: maps the names of network resources to their respective network addresses

AD: 
- When a user logs into a computer that is part of a Windows domain, Active Directory checks the submitted password and determines whether the user is a system administrator or normal user. Also, it allows management and storage of information, provides authentication and authorization mechanisms, and establishes a framework to deploy other related services.
- uses Lightweight Directory Access Protocol versions 2 and 3, Microsoft's version of Kerberos, and DNS

Azure AD:
- https://docs.microsoft.com/en-us/azure/active-directory/active-directory-whatis
- https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-developers-guide


#### Get start
1. get Azure AD tenant
  - domain: Azure AD tenant
  - ClientId: Application ID, Audience
  - user: zhenying.zhu91@hotmail.com
2. App registrations
  - TodoListService
  - TodoListWebApp
3. Click the app, setting properties
4. get Key: App key
5. Add permission

Build the app: https://github.com/Azure-Samples/active-directory-dotnet-webapp-webapi-openidconnect
- First build the app by using "ctrl+shift+B", then run with browser
- Use https://localhost:44322/ to sign in. Though the service running locally, when sign in, it will send a request to Azure AD and check the key.

API:
- Owin.IAppBuilder.UseWindowsAzureActiveDirectoryBearerAuthentication

ASP.NET:
- Controllers
- jQuery

ADAL:
- PCL: core
- WinRT: Windows Store
- CoreCLR: Core CLR


#### Template
https://github.com/Azure/azure-quickstart-templates