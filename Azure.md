# Azure

## Resource

- <https://www.tutorialspoint.com/microsoft_azure/index.htm>
- <https://docs.microsoft.com/en-us/azure/architecture/>
- <https://developer.microsoft.com/en-us/graph/blogs/30daysmsgraph-day-9-azure-ad-applications-on-v2-endpoint/>

## Azure Solutions Architect Expert

<https://docs.microsoft.com/en-us/learn/certifications/azure-solutions-architect?wt.mc_id=learningredirect_certs-web-wwl>

[Required Skills](https://query.prod.cms.rt.microsoft.com/cms/api/am/binary/RE3VJUW)

### Architect great solutions in Azure

<https://docs.microsoft.com/en-us/learn/paths/architect-great-solutions-in-azure/>

#### Pillars of a great Azure architecture

A great architecture

- Security: authN, data integrity.
- Performance and scalability: from service perspective, can handle peak traffic.
- Availability and recoverability: anticipates failure at all levels.
- Efficiency and operations: from cost and develop perspective. Need to have a good monitoring architecture, to see failures, resources.

Moving to the cloud introduces a model of shared responsibility. Responsibility stack:

- Data (SaaS covers till here)
- Apps
- Runtime (PaaS)
- Middleware
- O/S
- Virtualization (IaaS)
- Servers
- Storage
- Networking (On-premises)

Design for security

- customer data, business data, infra and dev ids need to be secured. Defense in depth and possible attacks per layer:
  - Data: expose encryption key
  - Applications: SQL injection and XSS
  - VM/compute: Malware
  - Networking: open unnecessary ports and brute-force attacks to gain access
  - Perimeter: DDoS
  - Policies & access: exposing credentials
  - Physical security: break into the office

Design for performance and scalability

- scale up (better machine) vs. scale out (more instances)
- need a service discovery mechanism to find active servers to send traffic
- consider network and storage
- an application performance management tool: uncover errors, poorly performing code, and bottlenecks in dependent systems.
- scalability and performance patterns
  - Data partitioning
  - Caching
  - Autoscaling
  - Decouple resource-intensive tasks as background jobs
  - Use a messaging layer between services: buffer
  - Implement scale units: a group of resources that are depend on each other. Scaling out can just add a new scale unit.
  - Performance monitoring

Design for availability and recoverability

- eliminating single points of failure
- availability == Service-Level Agreement (SLA)
- Clustering: can fail over between clusters
- Load balancing detect failed instances and prevent sending traffic to them
- recovery strategies from a possible data loss
- consider major downtime scenarios
- recovery point objective (RPO): max duration of acceptable data loss
- recovery time objective (RTO): the acceptable downtime duration

Design for efficiency and operations

- Efficiency: identifying and eliminating waste within your environment
- operational costs: wasted time and increased error
- PaaS services typically cost less than IaaS

#### Design for security in Azure

Zero Trust model

- you should never assume trust but instead continually validate trust.
- Input validation, output encoding, parameterized queries
- Key vault, DDoS protection, Threat detection by enabling logs, JIT access control, security testing and review
- CIA: the common principles used to define a security posture
  - Confidentiality: restrict access. least privilege. Protect passwords, certs, emails, etc.
  - Integrity: prevention of unauthorized changes. Protected by transmit the data with a fingerprint.
  - Availability: services are able to authZ users.

Security layers

- Data
- App
- Compute: secure access to VMs, patch system.
- Network: access control. Segmentation. restrict inbound traffic. secure connectivity.
- Perimeter: DDoS protection.
- Identity & Access: SSO, MFA. Audit events.
- Physical Security

Identity as a layer of security

- identity protocols used in internal network: Kerberos and LDAP
- Azure Active Directory: a cloud-based identity service. It has built-in support for synchronizing with your existing on-premises Active Directory or can be used stand-alone.
- SSO
  - access modifications are tied to the single identity
  - SSO can work with AAD.
  - Can also combine multiple data sources into an intelligent security graph.
  - Azure AD Connect is the tool to work with AAD.
- MFA
  - uses two of the something you know (password), something you possess (token generator), something you are (fingerprint)
  - AAD has free support for Global Admin to use MFA.
- Conditional access policies
  - AAD provides [conditional access policies](https://docs.microsoft.com/en-us/azure/active-directory/conditional-access/overview) (CAP): access policies based on group, location, device.
- Securing legacy applications
  - old way: authenticate to the on-prem admin application using Windows Integrated Authentication (WIA) from domain-joined machines, behind the corporate firewall.
  - new way: Azure AD Application Proxy. It can be used to publish apps, then users can use MyApps portal to auth.
- consumer identities
  - Azure AD B2C: an identity management service. It provides a social identity login experience.

Infrastructure protection

- ensure people and processes have only the rights they need
- Role-based access control
  - Security principals are mapped to roles directly or through group membership
  - Roles are sets of permissions
  - Roles can be granted at the individual service instance level, but they also flow down the Azure Resource Manager hierarchy.
  - Management groups add the ability to group subscriptions together and apply policy at an even higher level.
  - JIT access: flow roles through an arbitrarily defined subscription hierarchy
- Privileged Identity Management (PIM)
  - ongoing auditing of role members
  - manage, control, and monitor access to important resources in your organization
  - provide JIT access to Azure AD and Azure resources
  - Assigning time-bound access
  - Requiring approval to activate privileged roles
  - Enforcing Azure Multi-Factor Authentication (MFA) to activate any role
  - Using justification to understand why users activate
  - Getting notifications when privileged roles are activated
  - Conducting access reviews to ensure that users still need roles
  - Downloading an audit history for an internal or external audit

Providing identities to services

- through service principals and managed identities for Azure services
- Service principals
  - An identity is just a thing that can be authenticated
  - an account is data associated with an identity
  - A principal is an identity acting with certain roles or claims
  - a Service Principal is an identity that is used by a service or application. it can be assigned roles.
- Managed identities for Azure resources
  - can be instantly created for any Azure service that supports it
  - it creates an account on the Azure AD tenant
  - Azure infrastructure will automatically take care of authenticating the service and managing the account

Encryption

- two top-level types of encryption: Symmetric and Asymmetric. For Asymmetric encryption, both the public and private key can encrypt but cannot decrypt its own encrypted data. Need the paired key to decrypt.
- approached in two ways: encryption at rest and encryption in transit.
  - At rest: encrypted in the storage/physical medium.
  - In transit: encrypt before send or set up a secure channel.

[HTTPS](./Networking.md#HTTPS)

**HERE**: continue review

Encryption on Azure

- Azure Storage Service Encryption (SSE): data at rest. encrypts your data with 256-bit Advanced Encryption Standard (AES). can use Microsoft-managed encryption keys with SSE, or you can use your own encryption keys
- Azure Disk Encryption (ADE): protect the virtual hard disks (VHD). Uses BitLocker feature of Windows and the DM-Crypt feature of Linux. integrated with Azure Key Vault
- Transparent data encryption (TDE): protect Azure SQL Database and Azure Data Warehouse. real-time encryption and decryption.  using a symmetric key called the database encryption key. At rest and in transit.
- Azure Key Vault: a secure secrets store. vaults are backed by hardware security modules (HSMs).  centralizing the storage of application secrets,  control and log the access, renewing Transport Layer Security (TLS) certificates. secrets could be passwords, database credentials, API keys and, certificates.
- Azure Backup: encrypts local backups using AES256.

Network security

- limit exposure at the network layer across your services and systems.
- [All 7 OSI layers](https://en.wikipedia.org/wiki/OSI_model):
  1. Physical: Wire
  2. DataLink: WAN/LAN
  3. Network: IP
  4. Transport: TCP
  5. Session: RPC
  6. Presentation: TLS
  7. Application: HTTP
- Internet protection: only allow inbound and outbound communication where necessary.
  - Azure Security Center will identify internet-facing resources that don't have network security groups (NSG) and resources that are not secured behind a firewall.
  - Application Gateway: a Layer 7 load balancer. also includes a web application firewall (WAF) based on rules from the OWASP 3.0 or 2.2.9 core rule sets. Can prevent cross-site scripting and SQL injection.
  - network virtual appliances (NVA): protect non-HTTP-based services
  - Azure DDoS: distributed denial of service. notified using Azure Monitor metrics
- Virtual network (VNet) security: limit communication between resources to only what is required
  - network security groups (NSG). operate at layers 3 & 4.
  - Use VNET service endpoints can fully removing public internet access to resources.
- Network integration: provide improved communication between services in Azure.
  - Virtual private network (VPN): use ExpressRoute.
  - RDP and SSH are not permitted from internet endpoints

Application security

- Follow [Security Development Lifecycle](https://www.microsoft.com/en-us/securityengineering/sdl/) (SDL) which is a culture
- Operational security assessment: using Azure Security Center which is a Security vulnerability scanning software services
- Identity as the perimeter: use Azure AD and Azure AD B2C
- Data protection: use TLS and TDE. client-side encryption using .NET lib.
- Secure key and secret storage: use Azure Key Vault.

**HERE**: <https://docs.microsoft.com/en-us/learn/modules/design-for-performance-and-scalability-in-azure/2-scaling-up-and-scaling-out>

Next to look <https://docs.microsoft.com/en-us/azure/architecture/>

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

## Windows server manage certs

Add a cert:

1. Run mmc.exe
2. Go to File->Add/Remove Snap-in
3. Select Certificates in the Snap-in window and click Add.
4. Choose ‘Computer Account’ when prompted and click Finish.

[Disable a Root Certificate in Windows MMC](https://www.ssl.com/how-to/disable-a-root-certificate-in-windows-mmc/)

- Right click the cert in MMC
- Select "Disable all purposes for this certificate"

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

[Resolve RDP thindrives issue](https://github.com/neutrinolabs/xrdp/issues/720)

## Enable TLS 1.2

[doc](https://docs.microsoft.com/en-us/mem/configmgr/core/plan-design/security/enable-tls-1-2)

## repo security

Should not check in pfx.

## Azure template

<https://azure.microsoft.com/en-us/resources/templates/minecraft-on-ubuntu/>
