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

### Advancing Azure Active Directory availability

<https://azure.microsoft.com/en-us/blog/advancing-azure-active-directory-availability/>

### Proxy Addresses

<https://support.microsoft.com/en-us/help/3190357/how-the-proxyaddresses-attribute-is-populated-in-azure-ad>

ProxyAddress

- multi-value property contains addresses, like SMTP, X500, SIP, etc.
- objs in AD can have them. When sync to AAD, some rules applied and make them not same.

Concepts

- Initial domain: This is the first provisioned domain in the tenant. For example, contoso.onmicrosoft.com.
- Microsoft Online Email Routing Address (MOERA): The MOERA is constructed from the user's userPrincipalName attribute in Active Directory and is automatically assigned to the cloud account during the initial sync. For example, user@contoso.onmicrosoft.com. MOERA domain is init domain.
- Primary SMTP address: This is the primary email address of an Exchange recipient object. For example, SMTP:user@contoso.com. Notice it is different than the init domain.
- Secondary SMTP address: This is the secondary email address of an Exchange recipient object. For example, smtp:user@contoso.com.
- User principal name (UPN): The UPN can be the sign-in name of the user.
- mail: This is an attribute in Active Directory, the value of which represents the email address of a user.
- mailNickName: This is an attribute in Active Directory, the value of which represents the alias of a user in an Exchange organization.

### Service Principal

Azure Active Directory (Azure AD) is a centralized identity provider in the cloud. This capability is referred to as Single Sign On (SSO).

Azure AD authenticates users and provides access tokens. An access token is a security token that is issued by an authorization server. It contains information about the user and the app for which the token is intended. Azure AD uses JSON based tokens (JWTs) that contain claims.

Claim

- provides assertions about one entity, such as a client application or resource owner, to another entity, such as a resource server.
- name/value pairs that relay facts about the token subject.
- Applications can use claims to do AuthN and AuthZ
- contains
  - Security Token Server that generated the token
  - Date when the token was generated
  - Subject (such as the user--except for daemons)
  - Audience, which is the app for which the token was generated
  - App (the client) that asked for the token. In the case of web apps, this may be the same as the audience
- The token is signed by the Security Token Server (STS) with a private key. The STS publishes the corresponding public key. To validate a token, the app verifies the signature by using the STS public key to validate that the signature was created using the private key.
- An app can provide a refresh token to the STS, and if the user access to the app wasn't revoked, it will get back a new access token and a new refresh token.

App

- When you register your application with Azure AD, you are providing an identity configuration for your application that allows it to integrate with Azure AD.
- Customize the branding of your application
- This is a single tenant application (only users in the tenant), or a multi-tenant application (sign in using any work or school account).
- Request scope permissions for the app (which resource can the app access).
- Define new scopes that define access to your Web API.
- Share a secret/public key with Azure AD that proves the app's identity to Azure AD when the app is a confidential client application.
- Once registered, the application will be given a unique identifier. This id is used when request token from AAD.
- App can be a multiple tenants app. Service Principal is the instance of the app in a tenant.

Consent is the process of a resource owner granting authorization for a client application to access protected resources, under specific permissions, on behalf of the resource owner.

service principal: At deployment time, the Microsoft identity platform uses the application object as a blueprint to create a service principal, which represents a concrete instance of an application within a directory or tenant. The service principal defines what the app can actually do in a specific target directory, who can use it, what resources it has access to, and so on. The Microsoft identity platform creates a service principal from an application object through consent.

To access resources that are secured by an Azure AD tenant, the entity that requires access must be represented by a security principal. This is true for both users (user principal) and applications (service principal).

The security principal defines the access policy and permissions for the user/application in the Azure AD tenant.

[Terms](https://docs.microsoft.com/en-us/azure/active-directory/develop/developer-glossary)

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

More details about cert

[Public-key cryptography](https://en.wikipedia.org/wiki/Public-key_cryptography)

- public key: both the sender and the receiver have it. Used to encrypt the message.
- private key: only the receiver has it, used to decrypt.
- Public key infrastructure (PKI)

[Root and intermediate certs](https://www.thesslstore.com/blog/root-certificates-intermediate/)

- For SSL/TLS, server need install an SSL cert.
- root cert: it is the trusted root of the certificate chain. It is provided by root CA. On every device there is a root store, which contains root certs with their public keys. They are coming from either OS or browser.
- Root cert can sign other certs with its private key. In other word, root cert can issue other certs. Then those certs are trusted by the browser.
- end user/leaf certs (or other public trusted PKI certs) expired every 2 years. Root cert expired much longer.
- Each root CA can have different root certs for different purpose.
- To get an SSL cert issued, generate a Certificate Signing Request(CSR) and a private key. Then send the CSR to CA. CA sign it with the private key from the root and send it back.
- When visiting a website, browser check its cert's authenticity: validity date + digital signature + follow the cert chain + check if the cert is signed by a root cert in the root storage.
- Certification chain: to reduce the risk of the root CA being compromised, root CAs don't sign leaf certs using its private key. CA only issues intermediate roots, then use the private keys of intermediate root certs to sign and issue leaf certs.
-The server also install the intermidiate cert, and the browser can link the leaf cert to its root.
- Digital sign: notarization. Root cert transfer some trust to intermediate cert by digital sign it. The signature directly come from root cert's private key.
- Browser first get the leaf cert's public key, and then use it to verify the signature, and then use the chain to find its intermediate cert, and do the same validation until reaching root, or fail if it cannot chain to one of its trusted root.
- Inside a corp, the root CA is internal. The root cert + its public key is called key ceremony.
- SAN (subject alternate name) cert: a digital security cert which allows multiple hostnames to be protected by a single cert.
- In the cert there is a CRL (certificate revocation list) or OCSP (Online certificate status protocol). They are used to track if a cert was signed by the CA has been revoked the trust.
- certutil is very powerful.
- [Cert attributes](https://docs.oracle.com/cd/E24191_01/common/tutorials/authz_cert_attributes.html):
  - CN: CommonName
  - OU: OrganizationalUnit
  - O: Organization
  - L: Locality
  - S: StateOrProvinceName
  - C: CountryName
- Type of certs
  - Domain Validation (DV)
  - Organizational Validation (OV): the CA validate more things, such as also validate the location, then issue the cert.
  - Extended validation (EV): even further than OV.

[Get a cert use C#](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509store?view=netframework-4.8)

- Store has a name and a location.
- Location could be: CurrentUser, LocalMachine
- Name could be: My, Root

[OAuth](https://www.csoonline.com/article/3216404/what-is-oauth-how-the-open-authorization-framework-works.html)

- OAuth is an authz protocol/framework, unrelated services can allow authn access to their assets, without actually share the creds.
- seamless SSO (singal sign-on, one set of creds access multiple apps) amoung multiple computer.

## Create a Ubuntu Dev Desktop

[Install and configure Remote Desktop to connect to a Linux VM in Azure](https://docs.microsoft.com/en-us/azure/virtual-machines/linux/use-remote-desktop)

Install `xfce4` which is a lightweight desktop environment.

Install `xrdp`.

[How To Create a Sudo User on Ubuntu](https://linuxize.com/post/how-to-create-a-sudo-user-on-ubuntu/)

[Attach a data disk to a Linux VM](https://docs.microsoft.com/en-us/azure/virtual-machines/linux/attach-disk-portal)

[Resolve RDP thindrives issue](thindrive: https://github.com/neutrinolabs/xrdp/issues/720)