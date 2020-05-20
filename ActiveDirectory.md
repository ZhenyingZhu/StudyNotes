# Active Directory

## Resource

- [Windows Server 2012 Doc](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2012-r2-and-2012/hh831669(v=ws.11))
- <https://www.varonis.com/blog/active-directory-domain-services/>

## [Windows Server 2012 Doc](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2012-r2-and-2012/hh831669(v=ws.11))

Server Roles and Technologies

- Active Directory Certificate Services (AD CS): build a public key infrastructure (PKI).
- Active Directory Domain Services (AD DS): user and resource management, and provide support for directory-enabled applications such as Microsoft Exchange Server.
- Active Directory Federation Services (AD FS): provide secured identity [federation](https://en.wikipedia.org/wiki/Federated_identity) and SSO.
- Active Directory Lightweight Directory Services: a Lightweight Directory Access Protocol (LDAP). A thiner implementation of AD DS.
- Active Directory Rights Management Services (AD RMS): create reliable information protection solutions.
- Application Server: deploying and running custom, server-based business applications.
- Desktop Experience: Graphical Management Tools.
- Failover Clustering: manage VM and nodes failover.
- File and Storage Services.
- Group Policy.
- Hyper-V.
- Networking: for IT professional to design, deploy, and maintain Windows Server 2012.
- Network Load Balancing (NLB): enhances the availability and scalability of Internet server applications such as those used on web, FTP, firewall, proxy, virtual private network (VPN), and other mission-critical servers.
- Network Policy and Access Services: Network Policy Server (NPS), Health Registration Authority (HRA), and Host Credential Authorization Protocol (HCAP).
- Print and Document Services: including Print Server, Distributed Scan Server, and Fax Server.
- Remote Desktop Services: enables both a virtual desktop infrastructure (VDI) and session-based desktops.
- Security and Protection.
- Telemetry: send feedback to Microsoft.
- Volume Activation: deploy and manage volume licenses for a medium to large number of computers.
- Web Server (IIS).
- Windows Deployment Services: deploy Windows operating systems over the network.
- Windows Server Backup Feature.
- Windows Server Essentials Experience: such as simplified management using the server dashboard, data protection, Remote Web Access, and integration with Microsoft online services—all.
- Windows Server Update Services (WSUS).
- Windows System Resource Manager: manage server processor and memory usage with standard or custom resource policies. Help ensure that all the services provided by a single server are available on an equal basis.

### AD LDS

[Overview for 2012](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2012-r2-and-2012/hh831593(v=ws.11))

[Overview2 for 2008 and 2012](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/cc733064(v=ws.11)?redirectedfrom=MSDN)

- AD LDS provides data storage and retrieval for directory-enabled applications.
- provides much of the same functionality as AD DS, but it does not require the deployment of domains or domain controllers.
- Both AD LDS and AD DS build on the same core Microsoft directory service technologies.
- AD DS provides directory services for both the Windows server operating system and for directory-enabled applications. AD DS must adhere to a single schema throughout an entire forest.
- AD LDS can run multiple instances concurrently on a single computer, with an independently managed schema for each AD LDS instance.
- in environments where AD DS exists, AD LDS can use AD DS for the authentication of Windows security principals.
- A directory-enabled application uses a directory to hold data, rather than a database.
- Examples: customer relationship management (CRM) applications, human resource (HR) applications, and global address book applications.
- Directory services are optimized for read processing, while relational databases are optimized for transaction processing.
- directory services also provide such benefits as distributed architecture (multimaster design, replication, and geographical scalability); storage of identity data that is common to applications and platforms throughout an enterprise; flexible data schema; and fine-grained access policies.

A replica of an existing AD LDS instance contains replicated copies of the configuration and schema directory partitions, including any schema extensions.

LDAP Data Interchange Format (LDIF) files: importing schema definitions into an instance. transfer objects between Active Directory Domain Services (AD DS) and AD LDS partitions.

AD LDS server stores its database file and the associated log files in an instance-specific directory.

[Instance](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/cc753742(v=ws.11))

- service instance: a single running copy of AD LDS.
- each instance has a separate directory data store, a unique service name, and a unique service description.
- an instance can be part of a configuration set that provides replication of instance data to instances that run on separate servers.
- AD LDS provides
  - a hierarchical data store: adamntds.dit
  - a directory service component: dsmain.exe, adamdsa.dll
  - an client interfaces: LDAP, replication
- AD LDS does not require a domain controller or a Domain Name System (DNS) server.
- multimaster replication: grouping AD LDS instances into configuration sets.

**HERE**: <https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/cc753882(v=ws.11)>
