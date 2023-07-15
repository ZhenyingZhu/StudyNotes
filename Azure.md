# Azure

## Resources

In Plan

- <https://learn.microsoft.com/en-us/certifications/azure-fundamentals/>
- <https://learn.microsoft.com/en-us/certifications/azure-administrator/>
- <https://learn.microsoft.com/en-us/certifications/azure-solutions-architect/>

Backlog

- <https://www.tutorialspoint.com/microsoft_azure/index.htm>
- <https://developer.microsoft.com/en-us/graph/blogs/30daysmsgraph-day-9-azure-ad-applications-on-v2-endpoint/>
- <https://learn.microsoft.com/en-us/users/zhenyingzhu-8722/collections/bookmarks>
- <https://docs.microsoft.com/en-us/azure/architecture/>

## Content Map

Before [Old notes](#old-notes), capture systematic learning from certificates learning pathes. Each Certificate is a level 2 chapter, while a Learning Path is a level 3 chapter. Don't have too many more sub chapters in a learning path. Also don't take too much notes within a learning path.

## Microsoft Certified: Azure Fundamentals

<https://learn.microsoft.com/en-us/certifications/azure-fundamentals/>

### Learning Path: Microsoft Azure Fundamentals: Describe cloud concepts

<https://learn.microsoft.com/en-us/training/paths/microsoft-azure-fundamentals-describe-cloud-concepts/>

Cloud types

- Hybrid cloud: public + private cloud.
- Multi-cloud: different cloud providers.

expenses

- Capital expenditure
- Operational expenditure

### Learning Path: Azure Fundamentals: Describe Azure architecture and services

<https://learn.microsoft.com/en-us/training/paths/azure-fundamentals-describe-azure-architecture-services/>

```powershell
az version
az interactive
```

<https://portal.azure.com/>

- A region: across multiple DCs. Can have multiple AZs. But not all of regions have AZs.
  - Region pair: same geography (country) but far away. Can auto fail over.
  - Sovereign Regions
- An AZ: 1 or more DCs that are physically seperated. Belong to 1 region.
  - Zonal service
  - Zone-redundent service: the platform (e.g., zone redundent storage) replicates across AZs
  - Non-regional service: resilient to zone-wide outages as well as region-wide outages.

management infrastructure

- Resource group: can be grant access to
- subscription: authN, authZ, billing. Can have test, dev, prod subscriptions
  - billing boundary
  - access control boundary: can be based on env, or org structure
- account: an identity in Azure AD. Can have multiple subscriptions
- management group: manage many subscriptions
  - can apply a security policy
  - can apply Azure RBAC
  - can be nested

Virtual machine scale sets

- a group of identical, load-balanced VMs. Scale up/down automatically based on load or schedule. A LB is auto created.

Virtual machine availability sets

- a more resilient, highly available environment.
- stagger updates and have varied power and network connectivity
- update domain: can be rebooted at the same time. Bake 30 mins, then go to next update domain
- fault domain: same power source and network switch. An availability set is broken down into 3 FD.

lift and shift: move a physical server to the cloud

```powershell
az vm extension set # config extra software/service on a VM by using the Custom Script Extension
```

Azure Container

- lightweight virtual env, without OS. PaaS
- multiple containers on a single physical or virual host
- using microservice architecture
- Function can be stateful (Durable functions), a context is passed through

Azure Function

- an event driven, serverless compute option. No container is needed
- via an event (REST request), timer, or message

See logs: In the application insights > Logs

Azure App Service

- HTTP-based service for hosting web apps, REST APIs, WebJobs and mobile back ends
- enable auto deployments
- Endpoints can be secured
- auto scale and high availbility

Virtual Networking

- Isolation and segmentation: internal or external DNS server
- Internet communications
- Communicate between Azure resources: can via virtual network or service endpoints
- Communicate with on-premises resources
- Route network traffic: between subnets with route tables. Border Gateway Protocol (BGP)
- Filter network traffic: network security group (NSG) with inbound/outbound security rules. Network virtual appliances (e.g., firewall, WAN optimization)
- Connect virtual networks: virtual network peering privately

```powershell
IPADDRESS="$(az vm list-ip-addresses \
  --resource-group [resource group name] \
  --name my-vm \
  --query "[].virtualMachine.network.publicIpAddresses[*].ipAddress" \
  --output tsv)"

az network nsg list \
  --resource-group [resource group name] \
  --query '[].name' \
  --output tsv

az network nsg rule list \
  --resource-group [resource group name] \
  --nsg-name my-vmNSG
  --query '[].{Name:name, Priority:priority, Port:destinationPortRange, Access:access}' \
  --output table

az network nsg rule create \
  --resource-group [resource group name] \
  --nsg-name my-vmNSG \
  --name allow-http \
  --protocol tcp \
  --priority 100 \
  --destination-port-range 80 \
  --access Allow

curl --connect-timeout 5 http://$IPADDRESS
```

Azure Virtual Private Networks

- VPNs are typically deployed to connect two or more trusted private networks to one another over an untrusted network (typically the public internet).
- VPN gateway: deployed in a dedicated subnet and enable:
  - OnPrem DC to virtual network through site-to-site connection
  - device to virtual network through point-to-site
  - virtual network to virtual network through network-to-network
- Policy based: a set of IP addresses to encrypt packages.
- route based: IPSec tunnels are modeled as virtual network interface
- High-availability scenarios:
  - Active/standby
  - Active/active: both are taking traffic
  - ExpressRoute failover
  - Zone-redundant gateways: use both VPN gateways and ExpressRoute gateways

Azure ExpressRoute

- extend your on-premises networks into the Microsoft cloud over a private connection, with the help of a connectivity provider.

Azure DNS

- uses anycast networking
- supports private DNS domains
- supports alias record sets: refer to an Azure public IP address, an Azure Traffic Manager profile, or an Azure Content Delivery Network (CDN) endpoint

Azure storage accounts

- Redundant options
  - Locally redundant storage (LRS)
  - Geo-redundant storage (GRS): seconary region. Need customer to perform failover.
  - Read-access geo-redundant storage (RA-GRS): seconary only has read access
  - Zone-redundant storage (ZRS): 3 AZs in the primary region. Azure does failover.
  - Geo-zone-redundant storage (GZRS)
  - Read-access geo-zone-redundant storage (RA-GZRS)
- Types
  - Standard general-purpose v2: Blob Storage (including Data Lake Storage), Queue Storage, Table Storage, and Azure Files
  - Premium block blobs
  - Premium file shares: both Server Message Block (SMB) and network file system (NFS) file shares.
  - Premium page blobs
- Services
  - Blobs: text and binary data. Support for big data analytics. Has Hot/Cool/Archive access tier
  - Files: Server Message Block (SMB) or Network File System (NFS) protocols.
  - Queues: up to 64 KB in size.
  - Disks

Storage account endpoints

- Blob Storage: `https://<storage-account-name>.blob.core.windows.net`
- Data Lake Storage Gen2: `https://<storage-account-name>.dfs.core.windows.net`
- Azure Files: `https://<storage-account-name>.file.core.windows.net`
- Queue Storage: `https://<storage-account-name>.queue.core.windows.net`
- Table Storage: `https://<storage-account-name>.table.core.windows.net`

Azure Migrate: is a service that helps you migrate from an on-premises environment to the cloud.

Azure file movement options

- AzCopy: copy blobs or files to or from your storage account.
- Azure Storage Explorer
- Azure File Sync

Azure directory services

- in on-premises environments, Active Directory running on Windows Server provides an identity and access management service
- Online service subscribers: Microsoft 365, Microsoft Office 365, Azure, and Microsoft Dynamics CRM Online subscribers are already using Azure AD to authenticate into their account.
- Provides:
  - Authentication
  - Single sign-on (SSO)
  - Application management: Application Proxy, SaaS apps, the My Apps portal
  - Device management: devices to be managed through tools like Microsoft Intune. Allows for device-based Conditional Access policies

Azure Active Directory Domain Services (Azure AD DS)

- managed domain services such as domain join, group policy, lightweight directory access protocol (LDAP), and Kerberos/NTLM authentication
- support running legacy applications in the cloud that can't use modern authentication methods
- When create an Azure AD DS managed domain, needs to define a unique namespace: domain name. 2 Windows Server domain controllers are then deployed into the Azure region: a replica set.

Azure authentication methods

- authN methods: standard passwords, single sign-on (SSO), multifactor authentication (MFA), and passwordless
- SSO: only as secure as the initial authenticator
- MFA: authenticate elements fall into three categories: knows, has, is
- Passwordless: a device associated with an ID, plus knows and is.

External identities

- Business to business (B2B) collaboration
- B2B direct connect: another Azure AD tenant
- Azure AD business to customer (B2C)

Conditional access

- allow access to resources based on identity signals: who, where, what device.
- make dicisions to allow full or limited access

role-based access control

- The principle of least privilege
- a scope: a set of resources that an RBAC access control applies to.
- a scope can include: management group of subscriptions, single subscription, resource group, single resource
- typical roles: observers, users managing resources, admins, and automated processes
- when grant access at a parent scope, those permissions are inherited by all child scopes
- Azure RBAC doesn't enforce access permissions at the application or data level

zero trust model

- Verify explicitly: Always authenticate and authorize based on all available data points.
- Use least privilege access: Just-In-Time and Just-Enough-Access (JIT/JEA)
- Assume breach: Minimize blast radius and segment access. Verify end-to-end encryption. Use analytics to get visibility, drive threat detection, and improve defenses

defense-in-depth

- physical security layer: protect computing hardware in the datacenter.
- identity and access layer: SSO, access control, audit.
- perimeter layer: uses distributed denial of service (DDoS) protection
- network layer: limits communication between resources through segmentation and access controls.
- compute layer: secures access to virtual machines.
- application layer: helps ensure that applications are secure and free of security vulnerabilities.
- data layer: controls access to business and customer data that you need to protect.

Microsoft Defender for Cloud

- Azure-native service: services are monitored and protected without needing any deployment
- automatically deploy a Log Analytics agent to gather security-related data
- for on-prem/other clouds: Cloud security posture management (CSPM)
- 3 vital needs as you manage the security of your resources
  - Continuously assess
  - Secure: Zero Trust. **TODO**: See Azure Security Benchmark.
  - Defend

### Learning Path: Azure Fundamentals: Describe Azure management and governance

**TODO**: the cost.

## Microsoft Certified: Azure Administrator Associate

<https://learn.microsoft.com/en-us/certifications/azure-administrator/>

### Learning Path: AZ-104: Prerequisites for Azure administrators

```powershell
az find blob
az storage blob --help
```

Azure Resource Manager

- use a template
- provides a consistent management layer
- [Azure limits](https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/azure-subscription-service-limits)

Azure resource terminology

- resource provider: A service that supplies the resources. E.g., if you want to store keys and secrets, you work with the Microsoft.KeyVault resource provider.
- a resource type is in the format: {resource-provider}/{resource-type}.

Resource Group

- Resource group cannot be renamed
- All the resources in the group should share the same lifecycle
- Other than [resources not support move](https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/move-support-resources), resources can be moved across resource groups
- resource group region is where the metadata of the group stores
- can add a lock to prevent modify or delete.

ARM (Azure Resource Manager) template

- [Bicep](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/overview?tabs=bicep): a domain-specific language (DSL). Not in Json. When deploying, ARM run transpilation to make it become a JSON.
- parameters: need define value type
- [QuickStart templates](https://learn.microsoft.com/en-us/training/modules/configure-resources-arm-templates/6-review-quickstart-templates) can be good start points.

Administration tools

- Azure portal
- Azure CLI: `az vm create`
- Azure PowerShell: `New-AzVm`

Powershell commands

- `Get-Help -Name Get-ChildItem -Detailed`
- `Get-Module`
- `Install-Module -Name Az -Scope CurrentUser -Repository PSGallery`
- `Get-ExecutionPolicy`, `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser`
- `Connect-AzAccount`
- `Set-AzContext -Subscription '00000000-0000-0000-0000-000000000000'`
- `Get-AzResourceGroup | Format-Table`
- `$adminCredential = Get-Credential`

CLI commands

- `az find blob` to find most popular commands related to word blob
- `az storage blob --help`
- `az login`
- `az group list --query "[?name == '$RESOURCE_GROUP']" --output table`

ARM template

- uses a declarative syntax (outline resources without describing its control flow), not like imperative syntax which describe steps
- ARM templates are idempotent
- templates can be linked (nested). The linked templates should be stored and protected use SAS token
- Resource Manager orchestrates the deployment
- Can integrate ARM templates to CI/CD tools like [Azure pipeline](https://azure.microsoft.com/en-us/products/devops/pipelines/) or Github actions
- elements:
  - apiProfile: optional, define API version for resource types togather
  - parameters
  - variables
  - functions: reuse complicated expressions
  - resources: syntax `{resource-provider}/{resource-type}`. All the providers are listed in [Resource providers for Azure services](https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/azure-services-resource-providers). All the resources are sub pages of [Reference](https://learn.microsoft.com/en-us/azure/templates/)
  - output: values to return at the end of deployment
- deploy: `New-AzResourceGroupDeployment -Name $deploymentName -TemplateFile $templateFile  --parameters $key=$value`

ARM parameters

- input properties
  - type
  - defaultValue
  - allowedValues
  - minValue, maxValue
  - minLength, maxLength
  - metadata: description
- allowed types
  - string
  - object
  - integers
  - boolean
  - secureString, secureObject: cannot be read after deployment
  - array
- In ARM template: `"location": "[resourceGroup().location]",`, `"name": "[parameters('storageAccountType')]"`
- output properties
  - condition: bool whether to output it
  - type
  - value: `"value": "[reference(parameters('storageName')).primaryEndpoints]"`
  - copy: count, input when need to return more than 1

### Learning Path: AZ-104: Manage identities and governance in Azure

<https://learn.microsoft.com/en-us/training/paths/az-104-manage-identities-governance/>

Azure AD (Active Directory)

- directory and identity management service
- for On-Prem
  - [Kerberos](https://learn.microsoft.com/en-us/windows-server/security/kerberos/kerberos-authentication-overview): authentication protocol
  - [NTLM](https://learn.microsoft.com/en-us/windows-server/security/kerberos/ntlm-overview): a family of AuthN protocols
- for cloud
  - SAML
  - Oauth
  - Open ID
  - WS-Federation
- features
  - SSO
  - Ubiquitous device support: across different devices
  - Secure remote access: MFA, conditional access, group-based access
  - Cloud extensibility
  - Sensitive data protection: identity protection, audit
  - Self-service support: delegate tasks, JIT

Azure AD terms

- Identity: can be apps
- Account: an identity that has data associated with it
- Azure tenant (directory): An Azure tenant is a single dedicated and trusted instance of Azure AD
- Azure subscription: pay for Azure cloud services
- Active Directory Domain Services (AD DS): include Active Directory Certificate Services (AD CS), Active Directory Lightweight Directory Services (AD LS), Active Directory Federation Services (AD FS), and Active Directory Rights Management Services (AD RMS).

Azure AD

- Identity solution: designed for internet-based applications that use HTTPS communications
- Communication protocols: doesn't use Kerberos authentication. Can use SAML, WS-Federation, and OpenID Connect for authentication (and OAuth for authorization).
- Federation services: many third-party services like Facebook.
- Flat structure: no organizational units (OUs) or group policy objects (GPOs).
- Managed service: customers only manage only users, groups, and policies not devops.

Azure AD Editions

- Free: SSO, Core identity and access management, B2B collaboration.
- M365 Apps: includes branding
- Premium P1: hybrid id, group access management, conditional access
- Premium P2: id protection, governance

Azure AD join

- SSO
- Enterprise state roaming: sync user settings to joined devices

User accounts

- Cloud identity: can from an external Azure AD instance. Deleted in the primary directory deletes the user account.
- Directory-synchronized identity: defined in on-prem AD.
- Guest user: outside Azure like other cloud providers or Microsoft account (XBOX live).

Group accounts

- Security groups: manage access using security policy to shared resources.
- Microsoft 365 groups: provide access to a shared mailbox, calendar, files, SharePoint site, and more.
- can assign user to a group, or use Dynamic user/device

Administrative unit

- a set of permissions that can be assigned to roles

Products

- All azure products can be found in [Products available by region](https://azure.microsoft.com/en-us/explore/global-infrastructure/products-by-region/?products=all)

Azure subscription

- a logical unit of Azure services that's linked to an Azure account
- Billing for Azure services is done on a per-subscription basis.
- Every Azure subscription can be associated with an Azure AD.
- Can obtain through Enterprise agreement, reseller or partner.

Azure Policy

- Work on [management groups](https://learn.microsoft.com/en-us/azure/governance/management-groups/overview). By default, all new subscriptions are under root group.
- A policy definition describes the compliance conditions for a resource, and the actions to complete when the conditions are met.
- An initiative definition is a set of policy definitions that help you track your resource compliance state to meet a larger goal.
- [build in policies](https://learn.microsoft.com/en-us/azure/governance/policy/samples/built-in-policies)
- [built-in initiatives](https://learn.microsoft.com/en-us/azure/governance/policy/samples/built-in-initiatives)

Role-based access control

- control what areas of a resource each user can access.
- an authorization system built on Azure Resource Manager.
- Can manage an app/user/group to access resources and resource groups
- terms
  - Security principal: The object. User, group, service principal, managed identity
  - Role definition: A set of permissions that lists the allowed operations.
  - Scope: The boundary for the requested level of access. Root, management group, subscription, resource group, resource
  - Assignment: role def to a security principal

Role definition

- Actions: `Authorization/*/Delete`, `Authorization/elevateAccess/Action`
- NotActions
- DataActions
- AssignableScopes: `/`
- The system subtracts NotActions permissions from Actions permissions to determine the effective permissions for a role.
- A resource inherits role assignments from its parent resource.
- The effective permissions for a requestor are a combination of the permissions for the requestor's assigned roles, and the permissions for the roles assigned to the requested resources.

Type of roles

- Classic subscription administrator roles
- Azure role-based access control (RBAC) roles: Azure resources (irtual machines, SQL databases, storage)
- Azure Active Directory (Azure AD) administrator roles: manage Azure AD resources (users, groups, domains, billing, licensing, application registration). [Build in roles](https://learn.microsoft.com/en-us/azure/active-directory/roles/permissions-reference)

Azure AD Users

- Member users vs. Guest users
- Guest user: gets an invitation email that contains a redemption link or a direct link to an app you want to share

Azure AD Groups

- can manage cloud-based/on-premises apps, resources within the tenant or external (Saas apps, Azure services, Sharepoint sites, etc.)
- direct assign to a user, group assignment, rule based assignment

Azure AD B2B

- The guest can get the invitation through email. Or you can share the invitation to an application by using a direct link. The guest then redeems their invitation to access the resources.
- Giving access to external users is much easier than in a federation. A federation is where you have a trust established with another organization, or a collection of domains, for shared access to a set of resources.

Application

- [Azure AD organization > Manage > Enterprise applications](https://portal.azure.com/#view/Microsoft_AAD_IAM/StartboardApplicationsMenuBlade/~/AppAppsPreview/menuId~/null)
- For Azure AD P2 users, can add user/group

Azure RBAC

- grant access by assigning the appropriate Azure role to users, groups, and applications at a certain scope.
- The scope of a role assignment can be a management group, subscription, a resource group, or a single resource
- role assignment at a parent scope also grants access to the child scopes
- Access control/identity and access management (IAM)
- Azure RBAC is an allow model, but has `NotActions` permissions for subtract.
- In Activity logs service, can see RBAC audit logs.

SSPR

- Authentication: The user enters the required data to authenticate their identity. Can enforce at least n methods together.
  - Mobile app notification
  - Mobile app code
  - Email
  - Mobile phone
  - Office phone
  - Security questions

### Learning Path: AZ-104: Implement and manage storage in Azure

Azure Storage

- 3 categories of storages:
  - VM data: data disks
  - Unstructured data: data lake storage, blob storage
  - Structured data: Azure table, Cosmos DB, SQL
- 2 tiers: standard: HDD; premium: SSD
- 4 data services:
  - Azure Blob Storage (containers)
  - Azure Files: for deployments. Server Message Block (SMB) protocol and the Network File System (NFS) protocol.
  - Azure Queue Storage
  - Azure Table Storage (part of Cosmos DB): NoSQL (structured non relational). throughput-optimized tables, global distribution, and automatic secondary indexes. Handles capacity management with cost-effective serverless and automatic scaling options.
- storage account types:
  - Standard general-purpose v2: supports blob, queue, table, azure files
  - Premium block blobs: block storage
  - Premium file shares: Azure Files
  - Premium page blobs: Page blobs. storing index-based and sparse data structures, such as operating systems, data disks for virtual machines, and databases.
- are encrypted by using Storage Service Encryption (SSE) for data at rest.
- replication strategies:
  - Locally redundant storage (LRS): within a DC.
  - Zone redundant storage (ZRS): across 3 clusters in a region.
  - Geo-redundant storage (GRS): secondary region. Whether secondary region can be read depends on the choice.
  - Geo-zone-redundant storage (GZRS): 3 AZs in the primary region, and a seconary region.
- storage URLs:
  - container: `mystorageaccount.blob.core.windows.net`
  - table: `mystorageaccount.table.core.windows.net`
  - queue: `mystorageaccount.queue.core.windows.net`
  - file: `mystorageaccount.file.core.windows.net`

Secure storage endpoints

- Firewalls & Virtual networks. Subnets and virtual networks must exist in the same Azure region or region pair as your storage account.
- Go to access keys, then the connection string and key are there. It is same for queue.

Azure Blob Storage

- In container in a storage account.
- options
  - container: public access level: private, access to blob, access to container.
  - type: block (file), page (VM disks), append (log)
  - upload
  - access tier: hot, cool (for backup), archive (several hours latency).
  - data set lifecycle: auto check dates then move to different access tiers
  - object replication: From a source to a destination account. Requires blob versioning is enabled on both.
- upload tools: Azure Storage Explorer, AzCopy, Azure Data Box Disk (physical), Azure Import/Export
- pricing: Performance tiers, Access cost, transaction cost, Geo-replication transfer cost, Outbound data transfer costs, Changes to the storage tier.

**HERE**: <https://learn.microsoft.com/en-us/training/modules/configure-storage-security/>

## Microsoft Certified: Azure Solutions Architect Expert

<https://docs.microsoft.com/en-us/learn/certifications/azure-solutions-architect?wt.mc_id=learningredirect_certs-web-wwl>

[Required Skills](https://query.prod.cms.rt.microsoft.com/cms/api/am/binary/RE3VJUW)

### Learning Path: Build great solutions with the Microsoft Azure Well-Architected Framework

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

**TODO**: continue review

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

**TODO**: <https://docs.microsoft.com/en-us/learn/modules/design-for-performance-and-scalability-in-azure/2-scaling-up-and-scaling-out>

Next to look <https://docs.microsoft.com/en-us/azure/architecture/>

## Old notes

### Azure Key Vault

<https://learn.microsoft.com/en-us/azure/key-vault/general/tutorial-net-create-vault-azure-web-app>

### tutorialspoint Microsoft Azure Tutorial

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

[Microsoft Azure - Components](https://www.tutorialspoint.com/microsoft_azure/microsoft_azure_components.htm): **TODO**

### Active directory

name service: maps the names of network resources to their respective network addresses

AD:

- When a user logs into a computer that is part of a Windows domain, Active Directory checks the submitted password and determines whether the user is a system administrator or normal user. Also, it allows management and storage of information, provides authentication and authorization mechanisms, and establishes a framework to deploy other related services.
- uses Lightweight Directory Access Protocol versions 2 and 3, Microsoft's version of Kerberos, and DNS

Azure AD:

- <https://docs.microsoft.com/en-us/azure/active-directory/active-directory-whatis>
- <https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-developers-guide>

#### Advancing Azure Active Directory availability

<https://azure.microsoft.com/en-us/blog/advancing-azure-active-directory-availability/>

#### Proxy Addresses

<https://support.microsoft.com/en-us/help/3190357/how-the-proxyaddresses-attribute-is-populated-in-azure-ad>

ProxyAddress

- multi-value property contains addresses, like SMTP, X500, SIP, etc.
- objs in AD can have them. When sync to AAD, some rules applied and make them not same.

Concepts

- Initial domain: This is the first provisioned domain in the tenant. For example, contoso.onmicrosoft.com.
- Microsoft Online Email Routing Address (MOERA): The MOERA is constructed from the user's userPrincipalName attribute in Active Directory and is automatically assigned to the cloud account during the initial sync. For example, <user@contoso.onmicrosoft.com>. MOERA domain is init domain.
- Primary SMTP address: This is the primary email address of an Exchange recipient object. For example, <SMTP:user@contoso.com>. Notice it is different than the init domain.
- Secondary SMTP address: This is the secondary email address of an Exchange recipient object. For example, <smtp:user@contoso.com>.
- User principal name (UPN): The UPN can be the sign-in name of the user.
- mail: This is an attribute in Active Directory, the value of which represents the email address of a user.
- mailNickName: This is an attribute in Active Directory, the value of which represents the alias of a user in an Exchange organization.

#### Service Principal

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

### Get start

1. get Azure AD tenant

- domain: Azure AD tenant
- ClientId: Application ID, Audience
- user: <zhenying.zhu91@hotmail.com>

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

### Azure Template

<https://github.com/Azure/azure-quickstart-templates>

### Windows server manage certs

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

### Create a Ubuntu Dev Desktop

[Install and configure Remote Desktop to connect to a Linux VM in Azure](https://docs.microsoft.com/en-us/azure/virtual-machines/linux/use-remote-desktop)

Install `xfce4` which is a lightweight desktop environment.

Install `xrdp`.

[How To Create a Sudo User on Ubuntu](https://linuxize.com/post/how-to-create-a-sudo-user-on-ubuntu/)

[Attach a data disk to a Linux VM](https://docs.microsoft.com/en-us/azure/virtual-machines/linux/attach-disk-portal)

[Resolve RDP thindrives issue](https://github.com/neutrinolabs/xrdp/issues/720)

### Enable TLS 1.2

[doc](https://docs.microsoft.com/en-us/mem/configmgr/core/plan-design/security/enable-tls-1-2)

### repo security

Should not check in pfx.

### Azure template

<https://azure.microsoft.com/en-us/resources/templates/minecraft-on-ubuntu/>

- Notice the oracle java install is failing.

### Spring boot on Azure

<https://azure.microsoft.com/en-us/services/spring-cloud/#security>

### Azure Queue Storage

<https://learn.microsoft.com/en-us/azure/storage/queues/storage-queues-introduction>

<https://stackoverflow.com/questions/37428068/how-to-get-connection-string-to-existing-servicebus-without-old-azure-portal>
