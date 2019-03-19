# Azure

## Resource

<https://www.tutorialspoint.com/microsoft_azure/index.htm>

## tutorialspoint Microsoft Azure Tutorial

[Cloud Computing - Overview](https://www.tutorialspoint.com/microsoft_azure/cloud_computing_overview.htm)

Architecture of cloud computing

- Front-end device
- Back-end platform
- Cloud-based delivery
- Network

storage options

- Public
- Private
- Hybrid

Benefits of Cloud

- scalability
- reducing capital infrastructure
- access the application independent of their location and hardware configuration.
- simplifies the network
- more reliable

SPI

- SaaS: E-mail
- PAAS: Microsoft Azure
- IAAS: Amazon S3

[Microsoft Azure - Windows](https://www.tutorialspoint.com/microsoft_azure/microsoft_azure_windows.htm)

Azure as PaaS: The clients can focus on the application development rather than having to worry about hardware and infrastructure.

Azure as IaaS: gives complete control of the operating systems and the application platform stack to the application developers.

[Microsoft Azure - Components](https://www.tutorialspoint.com/microsoft_azure/microsoft_azure_components.htm)
HERE

## Portal

<https://portal.azure.com/>

<https://manage.windowsazure.com>

## Active directory

name service: maps the names of network resources to their respective network addresses

AD:

- When a user logs into a computer that is part of a Windows domain, Active Directory checks the submitted password and determines whether the user is a system administrator or normal user. Also, it allows management and storage of information, provides authentication and authorization mechanisms, and establishes a framework to deploy other related services.
- uses Lightweight Directory Access Protocol versions 2 and 3, Microsoft's version of Kerberos, and DNS

Azure AD:

- <https://docs.microsoft.com/en-us/azure/active-directory/active-directory-whatis>
- <https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-developers-guide>

## Get start

1. get Azure AD tenant

- domain: Azure AD tenant
- ClientId: Application ID, Audience
- user: zhenying.zhu91@hotmail.com

1. App registrations

- TodoListService
- TodoListWebApp

1. Click the app, setting properties
1. get Key: App key
1. Add permission

Build the app: <https://github.com/Azure-Samples/active-directory-dotnet-webapp-webapi-openidconnect>

- First build the app by using "ctrl+shift+B", then run with browser
- Use <https://localhost:44322/> to sign in. Though the service running locally, when sign in, it will send a request to Azure AD and check the key.

API:

- Owin.IAppBuilder.UseWindowsAzureActiveDirectoryBearerAuthentication

ASP.NET:

- Controllers
- jQuery

ADAL:

- PCL: core
- WinRT: Windows Store
- CoreCLR: Core CLR

## Template

<https://github.com/Azure/azure-quickstart-templates>

## Windows server add a cert

1. Run mmc.exe
2. Go to File->Add/Remove Snap-in
3. Select Certificates in the Snap-in window and click Add.
4. Choose ‘Computer Account’ when prompted and click Finish.

## Create a Ubuntu Dev Desktop

[Install and configure Remote Desktop to connect to a Linux VM in Azure](https://docs.microsoft.com/en-us/azure/virtual-machines/linux/use-remote-desktop)

Install `xfce4` which is a lightweight desktop environment.

Install `xrdp`.

[How To Create a Sudo User on Ubuntu](https://linuxize.com/post/how-to-create-a-sudo-user-on-ubuntu/)

[Attach a data disk to a Linux VM](https://docs.microsoft.com/en-us/azure/virtual-machines/linux/attach-disk-portal)