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

Azure Storage security strategies

- encryption: auto encrypted in the storage. Client-Side Encryption, HTTPS, or SMB 3.0. Can use microsoft-managed or customer managed keys.
- authentication: Azure AD and RBAC
- authorization
- user access control with credentials
- file permissions
- private signatures
- shared access signature (SAS): delegates access to a particular resource in your Azure storage account with specified permissions and for a specified time interval. A URI.
  - Account level vs. Service level
  - can also set store access policy
  - can specify IP and protocol
  - need to specify signing key
- Shared Key authorization: relies on your Azure storage account access keys and other parameters to produce an encrypted signature string
- Anonymous access to containers and blobs
- URI: Resource URI + Storage version (sv) + Storage service (ss) + Start time (st) + Expiry time (se) + Resource (sr) + Permissions (sp) + IP range (sip) + Protocol (spr) + Signature (sig)
- The Azure storage account and the key vault must be in the same region.

**HERE**: <https://learn.microsoft.com/en-us/training/modules/configure-azure-files-file-sync/>

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

[Microsoft Azure - Components](https://www.tutorialspoint.com/microsoft_azure/microsoft_azure_components.htm): **TODO**

## Old notes

### Azure services

<https://learn.microsoft.com/en-us/dotnet/azure/key-azure-services>

<https://learn.microsoft.com/en-us/dotnet/azure/migration/app-service>

### Network

[What is Azure Virtual Network?](https://learn.microsoft.com/en-us/azure/virtual-network/virtual-networks-overview)

- to communicate outbound with the internet: public ip, NAT gateway, public load balancer
- vnet service endpoint: limit traffic to storage/SQL to only a virtual network
- net peering: connect vnets.
- Network virtual appliances: a VM that performs a network function, such as a firewall or WAN optimization.

[Filter network traffic](https://learn.microsoft.com/en-us/azure/virtual-network/tutorial-filter-network-traffic)

- vnet: has subnets
- application security group (ASGs): group together servers with similar functions
- network security group (NSG): secures network traffic in your virtual network. Associate with subnet.
- secuity rule: source, dest, service, port, protocol, action (allow, deny), priority
- ASG can be used in security rules as service of a NSG
- VM NIC associates with a subnet in the vnet + ASG
- in the same subnet, traffic can always go through

[Route network traffic](https://learn.microsoft.com/en-us/azure/virtual-network/tutorial-create-route-table-portal)

- Custom route: route traffic between subnets through a network virtual appliance (NVA).
- IP forwarding: any traffic received by the NVA that's destined for a different IP address, isn't dropped and is forwarded to the correct destination.
- resource in private subnet can directly go to public subnet

[Restrict network access](https://learn.microsoft.com/en-us/azure/virtual-network/tutorial-restrict-network-access-to-resources)

- in the vnet, can enable service endpoint for azure resources, for example the storage
- NSG add dist for the storage service tag
- deny internet traffic
- storage add Firewall limit network access from internet

**TODO**: <https://learn.microsoft.com/en-us/azure/app-service/networking-features#regional-vnet-integration>

**TODO**: <https://learn.microsoft.com/en-us/azure/azure-functions/functions-networking-options?tabs=azure-portal#virtual-network-integration>

[Connect virtual networks](https://learn.microsoft.com/en-us/azure/virtual-network/tutorial-connect-virtual-networks-portal)

- create vnet peering by select Peering in the setting of vnet. Then 2 vnets are connected

[Best practices](https://learn.microsoft.com/en-us/azure/virtual-network/concepts-and-best-practices)

- Don't create subnets cover the entire address space of vnet, to plan for future.
- fewer large virtual networks rather than multiple small virtual networks to prevent management overhead

[Business Continuity](https://learn.microsoft.com/en-us/azure/virtual-network/virtual-network-disaster-recovery-guidance)

- serve as a trust boundary
- within the scope of a region

[Routing](https://learn.microsoft.com/en-us/azure/virtual-network/virtual-networks-udr-overview)

- [BGP vs. DNS](https://www.enterprisenetworkingplanet.com/data-center/bgp-vs-dns/): BGP: how, DNS: where
- Vnet peering: a route is auto created
- vnet gateway: a next hop type
- VirtualNetworkServiceEndpoint: when enabled, a route is added
- next hop types:
  - VirtualNetworkGateway
  - VNetLocal
  - Internet
  - VirtualAppliance
  - VNet peering
  - VirtualNetworkServiceEndpoint
  - None
- On prem can use BGP to connect to Vnet using ExpressRoute or VPN
- 0.0.0.0/0 address prefix is created as a default route with the Internet next hop type by Azure
- [ExpressRoute vs. VPN](https://medium.com/awesome-azure/azure-difference-between-azure-expressroute-and-azure-vpn-gateway-comparison-azure-hybrid-connectivity-5f7ce02044f3): ExpressRoute go through private network, VPN go through public
- Routes can be invalid if a same route is added

[Enable containers to use Azure Virtual Network capabilities](https://learn.microsoft.com/en-us/azure/virtual-network/container-networking-overview)

- A virtual network IP address is assigned to every Pod (one or more containers).
- Pods can connect to peered vnet.
- Pods can access services that are protected by vnet service endpoints.
- NSGs and routes can be applied directly to Pods.
- Pods can be placed directly behind an Azure internal or public LB, just like VMs
- Pods can be assigned a public IP. Pods can also access the internet themselves.
- Works seamlessly with K8s resources such as Services, Ingress controllers, and Kube DNS. A K8s Service can also be exposed internally or externally through the Azure LB.

[Peering](https://learn.microsoft.com/en-us/azure/virtual-network/virtual-network-peering-overview)

- Global virtual network peering: Connecting virtual networks across Azure regions.
- You can apply network security groups in either virtual network to block access to other virtual networks or subnets.
- can resize the address space of Azure virtual networks that are peered without incurring any downtime
- Service chaining enables you to direct traffic from one virtual network to a virtual appliance or gateway in a peered network through user-defined routes.
- Each virtual network, including a peered virtual network, can have its own gateway to connect to an on-premises network.
- Resources in one virtual network can't communicate with the front-end IP address of a basic load balancer (internal or public) in a globally peered virtual network.

[Integrate Azure services](https://learn.microsoft.com/en-us/azure/virtual-network/vnet-integration-for-azure-services)

- Use Private Endpoint that connects you privately and securely to a service powered by Azure Private Link. Private Endpoint uses a private IP address from your virtual network, effectively bringing the service into your virtual network. DNS resolution in the virtual network must be configured to resolve that same host name to the target resource's private IP address instead of the original public IP address
- Accessing the service using public endpoints by extending a virtual network to the service, through service endpoints.
- Using service tags to allow or deny traffic to your Azure resources to and from public IP endpoints.
- The Azure service fully manages service instances in a virtual network. This management includes monitoring the health of the resources and scaling with load.
- Certain services impose restrictions on the subnet they're deployed in. These restrictions limit the application of policies, routes, or combining VMs and service resources within the same subnet
- The client application typically uses a DNS host name to reach the target service.
- ASE (App service Environment)
- require a delegated subnet as an explicit identifier
- With service tags, you can define network access controls on network security groups or Azure Firewall.
- Service endpoints and private endpoints have characteristics in common. Private endpoint is individual instance.

<https://azure.microsoft.com/en-us/products/private-link>

<https://learn.microsoft.com/en-us/azure-stack/hci/concepts/software-load-balancer>

<https://learn.microsoft.com/en-us/azure/virtual-network/network-security-groups-overview#security-rules>

<https://learn.microsoft.com/en-us/azure/virtual-network/network-security-group-how-it-works>

### Service Tags

<https://learn.microsoft.com/en-us/azure/virtual-network/service-tags-overview>

<https://learn.microsoft.com/en-us/azure/azure-web-pubsub/howto-service-tags?tabs=azure-portal>

### DNS

- <https://learn.microsoft.com/en-us/azure/dns/dns-zones-records>
- <https://learn.microsoft.com/en-us/powershell/module/dnsserver/get-dnsserverzone?view=windowsserver2022-ps>
- <https://learn.microsoft.com/en-us/windows-server/networking/dns/manage-dns-zones?tabs=powershell>
- <https://medium.com/tech-jobs-academy/dns-forwarding-and-conditional-forwarding-f3118bc93984>
- <https://www.cloudflare.com/learning/dns/what-is-dns/#:~:text=DNS%20translates%20domain%20names%20to%20IP%20addresses%20so,which%20other%20machines%20use%20to%20find%20the%20device.>
- <https://docs.cleartax.in/cleartax-docs/general/how-to-check-dns-resolution>

- <https://learn.microsoft.com/en-us/azure/virtual-network/concepts-and-best-practices>

- DNS look-up request send through UDP on port 53.
- Host -> Local DNS -> Root DNS -> TLD DNS -> Authoritative DNS.
- Type: what name and value mean.
  - A: relay1.bar.foo.com 145.37.93.126, A
  - NS: foo.com dns.foo.com, NS. Also contain a A RR for the dns.
  - CNAME: foo.com relay1.bar.foo.com, CNAME. The canonical hostname.
  - MX: foo.com mail.bar.foo.com, MX. Canonical name for mail service.

On WinServer 2022

- Need to first add a static IP before enabling DNS

### DHCP

- <https://www.linkedin.com/advice/0/how-do-you-disable-dhcp-network-interface#:~:text=You%20can%20do%20this%20by,DHCP%20on%2C%20and%20select%20Properties.>

### Delete Azure endpoint

Error: Cannot delete custom domain "xxx" because it is still directly or indirectly (using "cdnverify" prefix) CNAMEd to CDN endpoint "xxx". Please remove the DNS CNAME record and try again.

<https://learn.microsoft.com/en-us/answers/questions/1102321/cannot-delete-custom-domain-because-it-is-still-di>

- doesn't work

you need to create a CNAME record with your DNS provider for

<https://learn.microsoft.com/en-us/azure/cdn/cdn-map-content-to-custom-domain?tabs=azure-dns%2Cazure-portal%2Cazure-portal-cleanup>

- The endpoint is CDN endpoint here. Need `Microsoft.CDN` registered.
- The domain is also appeared in the app service as a custome domain.

### Managed Identity

<https://learn.microsoft.com/en-us/azure/data-explorer/configure-managed-identities-cluster?tabs=portal>

<https://learn.microsoft.com/en-us/entra/workload-id/workload-identity-federation>

- Azure SDK can set the identity when login

Access policy and IAM, only 1 is needed.

Queue access: first create a queueServiceClient with the storage URI, then create a queueClient with the queue.

<https://learn.microsoft.com/en-us/azure/storage/files/authorize-data-operations-portal>

<https://learn.microsoft.com/en-us/azure/azure-portal/azure-portal-safelist-urls?tabs=public-cloud>

To use the new Queue API while still compatible with the old API for `byte[]` message, need to first init the client with an option. Because the old Queue API auto encode string to Base64.

```C#
   QueueServiceClient queueServiceClient = new (
      new Uri($"https://<queue name>.queue.core.windows.net/"),
      new DefaultAzureCredential(),
      new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });
```

Then use `Convert.ToBase64String(byte[])` to create the message, and `byte[] message = Convert.FromBase64String(dequeueMessage.Body.ToString()))` to deserialize a message back.

<https://learn.microsoft.com/en-us/azure/role-based-access-control/role-assignments-cli>

### Azure Key Vault

<https://learn.microsoft.com/en-us/azure/key-vault/general/tutorial-net-create-vault-azure-web-app>

- `az login --tenant <directory id>`
- `az account set --subscription <sub id>`
- `az group create --name "myResourceGroup" -l "EastUS"`
- `az appservice plan create --name myAppServicePlan --resource-group myResourceGroup --sku FREE`
- `az webapp create --resource-group "myResourceGroup" --plan "myAppServicePlan" --name "zhenyzhuakvwebapp" --deployment-local-git`, save the `deploymentLocalGitUrl`, something like <https://DeploymentUserZhenyzhu@zhenyzhuakvwebapp.scm.azurewebsites.net/zhenyzhuakvwebapp.git>.
- `az webapp config appsettings set -g MyResourceGroup --name "zhenyzhuakvwebapp" --settings deployment_branch=main`
- `az webapp config appsettings list -g MyResourceGroup --name zhenyzhuakvwebapp`: the app created in <https://zhenyzhuakvwebapp.azurewebsites.net>

- `az webapp identity assign --name "zhenyzhuakvwebapp" --resource-group "MyResourceGroup"`: [Creates a managed identity](https://portal.azure.com/#view/Microsoft_AAD_IAM/StartboardApplicationsMenuBlade/~/AppAppsPreview/menuId~/null), the object id is the principal id.
- `az keyvault set-policy --name "ZhenyingKeyVault" --object-id "<principalId>" --secret-permissions get list`: doesn't work. Error message: Cannot set policies to a vault with '--enable-rbac-authorization' specified.Need to change the access policy to make it not use RBAC.
- Or use the IAM UI and select managed identity. need to give the user admin permission. <https://stackoverflow.com/questions/69971341/unable-to-create-secrets-in-azure-key-vault-if-using-azure-role-based-access-con>

- `az webapp deployment user set --user-name "DeploymentUserZhenyzhu" --password "<pass>"` This user can use across tenants and is not a Azure AD user.
- `git init --initial-branch=main` create a local git repo
- `git remote add azure https://DeploymentUserZhenyzhu@zhenyzhuakvwebapp.scm.azurewebsites.net/zhenyzhuakvwebapp.git`

- `dotnet new web` create a empty .NET Core web app
- `dotnet add package Azure.Identity`
- `dotnet add package Azure.Security.KeyVault.Secrets`
- Use `DefaultAzureCredential` works both locally and remotely. Use `ManagedIdentityCredential` only remotely.
- `dotnet run` to confirm it can run locally. Need to add the user running the app to the Key Vault Access Policies.
- `git push azure main`
- Afterwards, if the local repo is gone, run `git clone https://DeploymentUserZhenyzhu@zhenyzhuakvwebapp.scm.azurewebsites.net/zhenyzhuakvwebapp.git` to get it back.

- Key Vault can use app to access. App can use client id + cert or client key

- Even in the same tenant, would need to use User assigned MI to use app auth

- For SQL: <https://learn.microsoft.com/en-us/azure/app-service/tutorial-connect-msi-sql-database?tabs=windowsclient%2Cefcore%2Cdotnet>
- For VM: <https://learn.microsoft.com/en-us/entra/identity/managed-identities-azure-resources/qs-configure-portal-windows-vm>

Need to add the identity to the User table.

<https://stackoverflow.com/questions/58313018/how-to-get-private-key-from-certificate-in-an-azure-key-vault>

<https://stackoverflow.com/questions/3708348/the-execute-permission-was-denied-on-the-object-xxxxxxx-database-zzzzzzz-s>

<https://stackoverflow.com/questions/31120912/how-to-view-the-roles-and-permissions-granted-to-any-database-user-in-azure-sql>

Local SQL: <https://stackoverflow.com/questions/66080953/unable-to-connect-to-localdb-mssqllocaldb-due-to-trigger-execution>

SN+I: <https://github.com/Azure/azure-sdk-for-net/issues/26655>

<https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/>

<https://learn.microsoft.com/en-us/entra/identity/managed-identities-azure-resources/tutorial-windows-vm-access-nonaad>

<https://azure.microsoft.com/en-us/products/event-hubs>

<https://learn.microsoft.com/en-us/virtualization/windowscontainers/manage-containers/container-base-images>

User Assigned Identity

- Create a service principal (enterprise app) and then assign to an AAD app.
- <https://learn.microsoft.com/en-us/entra/identity-platform/multi-service-web-app-access-storage?tabs=azure-portal%2Cprogramming-language-csharp>

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

### Web App Get start

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

#### Logs

Web app logs can be downloaded from <http://{mywebapp}.scm.azurewebsites.net/api/dump>. Resource: [Azure App Service Logging: How to Monitor Your Web Apps in Real-Time](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-app-service-logging-how-to-monitor-your-web-apps-in-real/ba-p/3800390).

- Works: <https://stackoverflow.com/questions/74503796/should-i-use-for-net-logging-in-azure-addazurewebappdiagnostics-or-addapplicat>
- How to fetch logs: <https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-8.0>
- Another approach: <https://stackoverflow.com/questions/78059505/log-streams-in-azure-not-picking-up-my-logs>

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

### Azure C# Client

The old nuget Microsoft.Azure.Management.Fluent is replaced by [Azure.ResourceManager](https://learn.microsoft.com/en-us/dotnet/azure/sdk/resource-management?tabs=PowerShell) and [Azure.Identity](https://learn.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme?view=azure-dotnet).

The AzureCredentials is managed by the AzureCredentialsFactory.FromServicePrincipal().

### Azure.Identity

[AuthN Scheme](https://developer.mozilla.org/en-US/docs/Web/HTTP/Authentication)

- server: challenge a client request
- client: provide authentication information
- Flow: 1. server return 401 with a WWW-Authenticate response header, 2. client respond an Authorization request header with the credentials, 3. server returns 200 or 401
- client can present a password prompt
- Proxy authentication: send 407 and `Proxy-Authorization` request headers, and wait for client to provide `Proxy-Authenticate` response.
- Access forbidden: return `401` for letting client provide AuthZ header, `403` after validation failed so client won't retry. `404` for hide the page.
- In the `Proxy-Authenticate` header, specify `<AuthN scheme type> realm=<realm>`. Realm is just a description.
- In the `Proxy-Authorization` header, specify `<AuthN scheme type> <credentials>`. Creds is encoded in base64.
- Schemes
  - Basic: user ID/password pairs, encoded using base64. Must use with TLS. On the server side, use `.htaccess` under the directory to protect to define the username
  - Bearer: access OAuth 2.0 protected resources with TLS. Tokens are issued to clients by an authorization server with the approval of the resource owner.
    - "before a client can access a protected resource, it must first obtain an authorization grant from the resource owner and then exchange the authorization grant for an access token.  The access token represents the grant's scope, duration, and other attributes granted by the authorization grant.  The client accesses the protected resource by presenting the access token to the resource server." From <https://datatracker.ietf.org/doc/html/rfc6750#section-1.1>

[OAuth 2.0 Authorization Framework](https://datatracker.ietf.org/doc/html/rfc6749), [A simplified doc](https://aaronparecki.com/oauth-2-simplified/)

- Roles: resource owner (user), resource server (API), client, authZ server (can be the same or different entity of resource server)
- Create app: reg the app with the service.
  - Must have a redirect URI to prevent attacks. Use HTTPS or custom URL scheme
  - Client id: build login URLs
  - Client secret: optional
- AuthZ: 4 grant types: authZ code, owner password credentials, client credentials (without user present), and implicit (when no client secret, but should not in use anymore)
- access token: a string denoting a specific scope, lifetime, and other access attributes. Add in the `Authorization` header to make request
- Web Server app flow:
  1. user gets grant by sending a request to AuthZ server with info in URI: `response_type=code` means want to get an authZ code, `client_id`, `redirect_uri`, `scope` of the resource, `state`: a random string generated by app for verify.
  2. User allows the grant
  3. User gets redirected to the API, with a response and the URI has `code=AUTH_CODE_HERE&state=1234zyx`
  4. API verify the state
  5. API exchange the AuthZ code to authZ server, with `grant_type=authorization_code`, `code=AUTH_CODE`, `redirect_uri=REDIRECT_URI`, `client_id=CLIENT_ID`, `client_secret=CLIENT_SECRET`
  6. AuthZ server replies `"access_token":"RsT5OjbzRn430zqMLgV3Ia"`, `"expires_in":3600` in the response body
- Web server app vs. SPA/Mobile app: SPA/Mobile app logic is in the Web browser/device which are out-of-control by the service, so cannot store secrets.

protocol flow:

1. client request authZ from the owner (preferable through authZ server)
2. client receive an authZ grant
3. client requests a token from authZ server by present the grant
4. authZ server authN the client and validate the grant, then provide the token
5. client use the token to get resource from resource server

Grant: a credential representing the resource owner's authorization.

- Code: the client directs the resource owner to an authorization server, which directs the resource owner back to the client with the authorization code (but not through owner's user agent). Resource owner only authN with authZ server.
- Implicit grant: simplified authZ code flow for JS. Directly issue access token without authZ code. The authZ server does not authN the client so need to weight against the security implications.
- owner password credentials: client shouldn't store it. Client should be on highly secured device.
- Client Credentials: need to authZ earlier.

Access Token

- opaque to the client.
- represent specific scopes and durations of access, granted by the resource owner, and enforced by the resource server and authorization server.
- denote an id. self-contain the authZ info. In verifiable manner. Has additional

- [HERE](https://datatracker.ietf.org/doc/html/rfc6749#section-1.5)

<https://learn.microsoft.com/en-us/entra/identity-platform/v2-overview>

- Components:
  - authentication service: OAuth 2.0/OpenID Connect standard-compliant
  - auth lib: MSAL
  - Application management portal: reg and config
  - App config API/CLI
  - developer content
- three types of identities:
  - Human identities
  - Workload identities
  - Device identities
- SSO: Once authenticated, the IAM system acts as the source of identity truth.
- AuthN: info in an ID token; AuthZ: info in an access token.
- AuthN: use OpenID Connect; AuthZ: use OAuth 2.0.
- Identity provider: creates, maintains, and manages identity information while offering authentication, authorization, and auditing services.
- identity and access management (IAM)
  - Identity management
  - Identity federation: SSO
  - Provisioning and deprovisioning of users
  - AuthN/AuthZ of users
  - Access control
  - Audit
- ID token: when AuthZ pass
- Access token: when consents are granted and AuthZ pass
- [AuthN/AuthZ standard](https://learn.microsoft.com/en-us/entra/fundamentals/introduction-identity-access-management?toc=%2Fentra%2Fidentity-platform%2Ftoc.json&bc=%2Fentra%2Fidentity-platform%2Fbreadcrumb%2Ftoc.json#authentication-and-authorization-standards)
  - OAuth 2.0: AuthZ
  - OpenID Connect (OIDC): AuthN
  - JSON web tokens (JWTs)
  - Security Assertion Markup Language (SAML): AuthN
  - System for Cross-Domain Identity Management (SCIM)
  - Web Services Federation (WS-Fed)
  - Active Directory Federation Services (AD FS): identity provider
- Auth flows: 4 parties
  - Identity provider/AuthZ server
  - Client
  - Resource owner
  - Resource server
- principal: user/host/service
- Bearer token: in entra, it is JWT standard. 3 types:
  - Access: issued by id provider, pass to resource server
  - ID: issued by id provider, signing in user and get user info
  - Refresh: client send it to IdP for new tokens. Is sensitive.
- App reg: needs below settings
  - app id = client id
  - redirect uri: idP use it
  - endpoints: 1. AuthZ, 2. token
- Sec token for app types: web app, mobile, desktop, web API, IoT
  - SPA: token acquired by a JS app in browser.
  - Public client app: with a user
  - Confidential client applications: without a user
- Sign-in audience: work/school or personal account
- desktop app:
  - interactive token-acquisition methods of MSAL: uses a web browser
  - computers joined either to a Windows domain or by Microsoft Entra ID: integrated win authN
  - a device without a browser: must sign in on another device that has a web browser
  - if you want the token cache to persist, you can customize the token cache serialization.
- Protected web API:
  - use access token to secure the API's data and authenticate incoming requests.
  - appends an access token in the authorization header of an HTTP request.
  - use the ASP.NET JWT middleware, i.e., IdentityModel extensions (not MSAL.NET) to validate the access token.
- Web API that calls another web API on behalf of a user: need to needs to acquire a token for the downstream web API and provide custom cache serialization.
- Daemon app that calls a web API: app can authenticate and get tokens by using the app's identity. The app proves its identity by using a client secret or certificate add to the app registration in Microsoft Entra ID. such secrets include application passwords, certificate assertion, and client assertion. using the client credential acquisition methods in MSAL.
- Security tokens:
  - Access token: contains info about the user and the resource. Resource needs to validate token.
  - refresh token: client app exchange it with AuthZ servers
  - ID token: Client to AuthN the user.
- Validate token: the app validates the token. AuthZ server signs the token with a private key and publish a public key. App uses the public key to verify the signature.
- JWTs and claims: AuthN. A claim provides assertions about 1 entity (e.g., client app, resource owner) to another entity (resource server). A bunch of facts.

Access token

- access tokens are opaque strings without a set format. Guid or encrypted blobs.
- JWT: can be decoded using a site like jwt.ms. Access token is part of JWT.
- Token ownership: resource is the audience (claim `aud`)
- Validate token:
  - Web APIs must validate access tokens sent to them by a client. They must only accept tokens containing one of their AppId URIs as the `aud` claim.
  - Web apps must validate ID tokens sent to them by using the user's browser in the hybrid flow, before allowing access to a user's data or establishing a session.
  - Other scenarios don't need to validate token because they directly talk with IDP to make sure ID token is valid.
  - Validate an ID token or an access token, it should first validate the signature of the token and the issuer against the values in the OpenID discovery document.
- Validate the issuer: The Issuer Identifier MUST exactly match the value of the issuer Claim `iss` and `tid`
- Validate the signature: JWT segment 1: header, 2: body, 3: signature. Header contains `alg` and `kid` for the public key
- [code ref](https://learn.microsoft.com/en-us/entra/identity-platform/access-tokens#recap)

Scenarios:

- all: reg app on [Entra admin center](https://entra.microsoft.com/#home), at Identity > Applications > App registrations
- add login scopes that client use: prompt for user to consent. e.g., `User.Read`
- SPA: use JS MSAL with Proof Key for Code Exchange (PKCE).
- Web app: Use self signed cert.
  - Can create a test cert using `dotnet dev-certs https -ep ./certificate.crt --trust`.
  - In the appsettings.json, add info to use Azure AD.
- Web API: Need to add a scope.
  - In code, add `[RequiredScope]` annotation. Add `AddAuthentication` middleware. `app.UseAuthentication(); app.UseAuthorization();`
- Desktop app (electron): use JS
  - Need middleware `AddAuthentication` and `AddMicrosoftIdentityWebApi(AzureAd)`
- Andriod: Use Java and `vsts-maven-adal-android`
- Daemon: use a secret.
- [HERE]<https://learn.microsoft.com/en-us/entra/identity-platform/quickstart-web-api-aspnet-core-protect-api>

<https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/README.md>

- Credentials: client use creds to authN requests to service.
- OAuth with Entra ID.
- [Cred classes](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/README.md#credential-classes)
  - Azure-hosted apps: Chained, Env, MI
  - SP: assert, cert, secret
  - user: AuthZ code, device code, interactive browser, on behalf of, username password
- [Managed identity](https://learn.microsoft.com/en-us/entra/identity/managed-identities-azure-resources/overview) a.k.a, MSI
  - System-assigned: can be used for VM. Creates a service principal.
  - User-assigned
  - [Use a Windows VM system-assigned managed identity to access Resource Manager](https://learn.microsoft.com/en-us/entra/identity/managed-identities-azure-resources/tutorial-windows-vm-access-arm)
  - `$response = Invoke-WebRequest -Uri 'http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&resource=https://management.azure.com/' -Method GET -Headers @{Metadata="true"}`

### Acquire token

```C#
string uriString = "<service uri>";

AADAuthURI = "https://login.microsoftonline.com/microsoft.com";
AuthenticationContext authContext = new AuthenticationContext(AADAuthURI);
ClientCredential applicationCredentials = new ClientCredential(clientAppID, appSecret);
AuthenticationResult result = authContext.AcquireTokenAsync(uriString, applicationCredentials).GetAwaiter().GetResult();

WebRequest request = WebRequest.Create(new Uri(uriString));
request.Headers.Set(HttpRequestHeader.Authorization, string.Format(CultureInfo.InvariantCulture, "{0} {1}", "Bearer", bearerToken));
```

<https://learn.microsoft.com/en-us/entra/identity-platform/msal-acquire-cache-tokens>

### MFA

<https://learn.microsoft.com/en-us/entra/identity/authentication/concept-mandatory-multifactor-authentication>

- <https://entra.microsoft.com/#home>
- <https://admin.microsoft.com/>: buy products

### VM

The Azure Spot discount has limit SKUs available.

To check SKU availability in region and zone, run `az vm list-skus --location centralus --size Standard_D --all --output table`

### Azure Resource Manager

<https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/resourcemanager/Azure.ResourceManager/docs/MigrationGuide.md>

Microsoft.Azure.Management.Compute

- <https://stackoverflow.com/questions/35228042/how-to-create-serviceclientcredential-to-be-used-with-microsoft-azure-management>

### Azure Pipelines

<https://learn.microsoft.com/en-us/azure/devops/pipelines/get-started/what-is-azure-pipelines?view=azure-devops>

<https://learn.microsoft.com/en-us/azure/devops/pipelines/artifacts/pipeline-artifacts?view=azure-devops&tabs=yaml>

To deploy ARM, go to Automation > Export Template > Deploy.

### Event Grid

<https://learn.microsoft.com/en-us/azure/event-grid/overview>
