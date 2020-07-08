# Active Directory

## Resource

- <https://www.varonis.com/blog/active-directory-domain-services/>
- [AD LDS Old Doc](https://docs.microsoft.com/en-us/previous-versions/windows/desktop/adam/active-directory-lightweight-directory-services)

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

[AD LDS Schema](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/cc753882(v=ws.11))

- Each AD LDS configuration set has its own independently manageable schema, which is stored in the schema directory partition.
- the base (or default) AD LDS schema contains only the classes and attributes that are needed to start an AD LDS instance.
- The schema can be extended with new classes and attributes, either by administrators or by the applications themselves.
- Can define either Object classes or Attributes
- Both single-valued and multivalued attributes can be indexed. classes cannot be indexed.
- wildcards search string can only used on indexed attributes

[AD Schema and all pre-defined](https://docs.microsoft.com/en-us/windows/win32/adschema/active-directory-schema)

- property == attribute.
- 3 distinct categories of classes: Structural Classes, Abstract Classes, and Auxiliary Classes.
- Content rules: expressed by the Must-Contain and May-Contain attributes of the schema definitions for each class.
- Directory Information Tree (DIT): the class instances stored in a tree structure
- Lightweight Directory Access Protocol (LDAP): Internet communications protocol.
- Object: a Class-Schema object and a group of Attribute-Schema
- Object Identifier ((OIDs)): uniquely identify data elements, syntaxes, and various other parts of distributed applications. a dotted string of numbers, like "1.2.840.113556.1.5.4
- Schema: defines the Poss-Superiors (defines what classes can be parents), Must-Contain, and May-Contain attributes.
- Security Descriptor: ownership of an object and the permissions
- Structure Rules: expressed by the Poss-superiors
- X.500: specify the naming, data representation, and communications protocols for a directory service.

- objectClass: can be classSchema, attributeSchema
- cn: the schame name
- Dn == distinguishedName, a chain of cns.

[Schema related info in AD Doc](https://docs.microsoft.com/en-us/windows/win32/ad/active-directory-schema)

- Each attribute is described by an attributeSchema
- definition: includes a variety of data, i.e. what object types that the attribute applies to; the syntax type of the attribute.
- Domain-replicated, stored attributes: some attributes are stored in the directory and replicated to all domain controllers in a domain, like [cn](https://docs.microsoft.com/en-us/windows/win32/adschema/a-cn), [objectGUID](https://docs.microsoft.com/en-us/windows/win32/adschema/a-objectguid). A subset of these attributes is also replicated to the global catalog.
- Non-replicated, locally stored attributes, such as badPwdCount, Last-Logon, and Last-Logoff are stored on each domain controller, but are not replicated. to obtain the last time that a user logged on to the domain, retrieve the Last-Logon attribute for the user from every domain controller in the domain and find that latest date and time.
- Non-stored, constructed attributes: calculated by the domain controller

Hierarchy

- every object instance except the root is contained by other object.
- except classSchema or attributeSchema, any object can be a container. possSuperiors attribute defines the container of the object class.

Object Identities

- Relative Distinguished Name: rDnAttID. Normally use  cn (Common-Name) as the naming. The value should be unique in a container, but not accross containers.
- Distinguished Name: distinguishedName. a string that includes the location of the object. relative distinguished name + each of its ancestors. unique within a forest.
- Object GUID: objectGUID. a globally unique identifier. Object GUIDs never change even moved.

[Naming Contexts and Directory Partitions](https://docs.microsoft.com/en-us/windows/win32/ad/naming-contexts-and-partitions)

- Directory partitions are also known as naming contexts.
- Schema Partition: contains classSchema and attributeSchema objects
- Configuration Partition
- Domain Partition
- Application Directory Partition

**TODO**: <https://docs.microsoft.com/en-us/windows/win32/ad/naming-contexts-and-partitions>

**TODO**: <https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/cc770465(v=ws.11)>

## Active Directory, 5th Edition

[Oreilly book](https://learning.oreilly.com/library/view/active-directory-5th/9781449361211/pr02.html)

### Chapter 1. A Brief Introduction

network operating system (NOS): a networked environment, many resources, managed by admin, accessed by end users.

Lightweight Directory Access Protocol (LDAP): X.500, define the standards for how a true directory service is implemented and accessed.

domain: group resources based on administrative and security boundaries

domain controllers: servers providing the NOS services to end users

directory service: a repository of network, application, or NOS information that is useful to multiple applications or users.

### Chapter 2. Active Directory Fundamentals

#### 2.1. How Objects Are Stored and Identified

Type of objects at structural level

- Container
- non-container, a.k.a, leaf node

The data is still sotred in a flat database. The directory information tree (DIT) file is an Extensible Storage Engine (ESE) database file.

organizational unit (OU). It is also a prefix, and is most commonly used as a container. `cn=Keith Cooper,ou=Northlight IT Ltd,dc=mycorp,dc=com`

Each object has a globally unique identifier (GUID). It is a 128-bit number. It is not changed even the object is moved or renamed within the DIT, until the obj is deleted and the GUID is released

Microsoft Active Directory Migration Tool (ADMT): cross-forest move of a security principal. Not perserve GUID.

Each obj also has a distinguished name (DN), same as Hierarchical path, and is uniq. domain mydomain.mycorp.com represents as DN: `dc=mydomain,dc=mycorp,dc=com`. dc stands for domain component

A relative distinguished name (RDN): reference an object within its parent container. DN: `cn=Administrator,cn=Users,dc=mycorp,dc=com`, RDN: `cn=Administrator`

[Attribute types from RFC 2253](https://learning.oreilly.com/library/view/active-directory-5th/9781449361211/ch02.html#attribute_types_from_rfc2253). [RFC 2253](https://www.ietf.org/rfc/rfc2253.txt)

#### 2.2. Building Blocks

An Active Directory domain is made up of the following components:

- X.500-based hierarchical structure
- A DNS domain name as a unique identifier
- A security service do authZ and authN in the domain
- Policies do ACL for users or machines within the domain

A domain controller (DC) can be authoritative for one and only one domain.

a domain tree: root domain connected with sub domains.

- all the domains in a domain tree trust one another implicitly with transitive trusts.
- Trust relationship just setup potential to allow access. Admin still need grant access.

Forest

- a collection of one or more domain trees.
- trees share a common Schema and Configuration container
- trees connected together through transitive trusts
- named as the forest root domain
- Seems like the domains under forest cannot be islodated from each other, so be careful to use forest

OU

- The primary type of container to house objects
- other than another type of container, which is called `container`, OU can have group policies
- some default containers and organizational units: Users, Computers containers, the Domain Controllers OU.
- Can use Active Directory Users and Computers (ADUC) MMC snap-in to manage

Global Catalog (GC)

- perform forest-wide searches.
- contains a subset of attributes for each object to search for. partial attribute set (PAS). Use Active Directory Schema snap-in to manage. Or modifying the `attributeSchema` object.
- be accessed via LDAP over port 3268 or LDAP/SSL over port 3269.
- in multidomain forest, first query against GC to find the domain, then query against the domain controller

Flexible Single Master Operator (FSMO) Roles

- Active Directory is a multimaster directory
- Active Directory nominates one server to act as the master for 5 functions which can only perform on a single domain controller
- The master server is FSMO role owner
  - Schema master (forest wide): can update the schema. in `cn=Schema,cn=Configuration,dc=mycorp,dc=com`
  - Domain naming master (forest): controls changes to the namespace, add/remove/rename domains, create app partitions and replicas. in `cn=Partitions,cn=Configuration,dc=mycorp,dc=com`
  - primary domain controller (PDC) emulator (domain): maintain the latest password for any account. In `dc=mycorp,dc=com`
  - Relative identifier (RID) master (domain): security principal has a security identifier (SID), comprised by RID. in `cn=RID Manager$,cn=System,dc=mycorp,dc=com`
  - Infrastructure master (domain): maintain references to objects in other domains, a.k.a phantoms. performing updates to the domain. in `cn=Infrastructure,dc=mycorp,dc=com`
- If the domain controller cannot perform FSMO, then a referal will be returned
- FSMO roles can be transferred between domain controllers

Time Synchronization in Active Directory

- Kerberos, authentication protocol for Active Directory clientsm uses system clocks to verify the authenticity of Kerberos packets.
- AD uses a time synchronization system based on the Network Time Protocol (NTP)

Domain and Forest Functional Levels

- expanded on the domain mode concept. apply to both forests and domains.
- dictate what types of operating systems can assume the role of a domain controller in a domain or forest. i.e., This windows OS supports what domain controller OS.

Groups

- three group scopes: domain local, domain global, and universal
- each group scope can have two types: distribution and security.
- Distribution groups are generally used as a messaging list/email list
- Security groups can be leveraged as distribution lists

### Chapter 3. Active Directory Management Tools

#### 3.1. Management Tools

Microsoft Management Console (MMC) snap-ins

Active Directory Administrative Center (ADAC)

- Runs PowerShell cmdlets, connect to the Active Directory Web Service (ADWS)
- `New-ADOrganizationalUnit`, `Set-ADObject`, `Move-ADObject`

Active Directory Users and Computers (ADUC)

- Can pass in LDAP filters to search

ADSI Endit

- a Resource Kit tool
- browse all of the attributes of an object
- Backlink attributes are constructed and not changeable.

LDP

- connect to any LDAP server (even non-Microsoft platforms), perform searches, view data, and make modifications.
- View data in a raw format
- start in command prompt
- bind means connect to the LDAP server with creds.
- Can issue an LDAP query in the Search dialog
- Can specify all attributes to return, or just pass in `*` to return all
- Can modify an object
  - Replace: update a normal attribute
  - Add: append a value to a multivalued attributed
  - Delete: clear an attribute or remove a specific value from a multivalued attribute
- Can create, delete, rename, and view replication metadata, view the ACL/security descriptor

#### 3.2. Customizing the Active Directory Administrative Snap-ins

Mostly talking about how to update the view of ADUC.

#### 3.3. Active Directory PowerShell Module

[Powershell Cookbook](http://oreil.ly/Win_PowerShell_CB2)

#### 3.4. Best Practices Analyzer

dcdiag tool can check the health of a domain controller, domain or forest and troubleshooting.

Best Practices Analyzer (BPA) is the future of dcdiag.

```powershell
Invoke-BpaModel "Microsoft/Windows/DirectoryServices"
Get-BpaResult "Microsoft/Windows/DirectoryServices"

Get-BpaModel | ft Id,Name
```

#### 3.5. Active Directory-Based Machine Activation

key management server (KMS): active windows

Volume Activation Management Tool 3.0 (VAMT)

Windows Automated Deployment Kit (ADK)

### Chapter 4. Naming Contexts and Application Partitions

naming context (NC): a domain as a big data partition. Only domain controllers that are authoritative for a domain need to replicate all of the information within that domain.

predefined naming contexts

- Domain naming context, scope to domain: e.g., users, groups, computers
- Configuration naming context, forest: e.g., LDAP policies, sites, subnets
- Schema naming context, forest

application partitions: user-defined partitions

- can contain any type of object except for security principals
- can define which domain controllers replicate the data contained within these partitions
- not restricted by domain boundaries
- can view naming contexts and application partitions by querying its `RootDSE` entry

#### 4.1. Domain Naming Context

DN of the root of this NC: `dc=mycorp,dc=com`. a.k.a, NC head.

Has a container `cn=LostAndFound`. contains Orphaned objects, they were created in a container that was deleted from another domain controller within the same replication period.

#### 4.2. Configuration Naming Context

every writable domain controller in the forest holds a writable copy of the Configuration NC.

#### 4.3. Schema Naming Context

schema is defined on a forest-wide basis. the Schema NC is writable only on the domain controller holding the schema master FSMO role.

Schema modifications need to be processed prior to any updates that utilize the schema.

it is a single container that has `classSchema`, `attributeSchema`, and `subSchema` objects.

#### 4.4. Application Partitions

Can  create areas in Active Directory to store data on specific domain controllers.

replica: a domain controller hold a copy of an application partition

site topology: can be used to automatically create the necessary connection objects to replicate among the servers that hold replicas of an application partition.

Domain controllers register Service Location (SRV) records to let clients use the DC locator.

DN of an application partition: if the partition calls `apps`, then DN is `dc=apps,dc=mycorp,dc=com`

Use `ntdsutil` to create an app partition.

```powershell
create nc "dc=MyAppPart,dc=cohovines,dc=com" dc01.cohovines.com
```

Application partitions tend to store dynamic data, which has a limited lifespan, i.e, has a time-to-live (TTL) value, e.g., DNS, DHCP

Create a dynamic object: add `dynamicObject` to the `objectClass` when create the object. Also can add `entryTTL`.

Dynamic objects do not get tombstoned like normal objects when they are deleted.

### Chapter 5. Active Directory Schema

- class
- attribute
- syntax: define the type of data that can be placed into an attribute.

To check the `schemaVersion`: `adfind -schema -s base objectVersion`. This is the version of AD default schema.

#### 5.1. Structure of the Schema

Schema container: `cn=schema,cn=Configuration,dc=contoso,dc=com`

Schema is made up of: classes `classSchema` and attributes `attributeSchema`

X.500 and the OID Namespace

- individual object classes in an organization can be uniquely defined using a special identifying process
- classes can inherit from one another
- need to define and export a class
- object identifier (OID): identify every schema object. e.g., `1.3.6.1.4.1.3385.12.497`. Those numbers are branchs
- Internet Assigned Numbers Authority (IANA) maintains the main set of root branches

#### 5.2. Attributes (attributeSchema Objects)

`attributeSchema` class inherits attributes from the class called `top`

Dissecting an example active directory attribute

- `userPrincipalName` (UPN): identifying each user across a forest. Can be used to login.
- When write, AD doesn't prevent dup UPN. domain controllers will log an event from source Key Distribution Center (KDC) when detect dup UPN
- each attribute in an `attributeSchema` has
  - lDAPDisplayName
  - syntax: e.g., `CASE_IGNORE_STRING`, `DN_STRING`
  - value: the diff from ldapDisplayName is that multiple attributes can have same values but different names
- if `searchFlags` has value 1, means it is indexed.

#### 5.3. Attribute Properties

Attribute Syntax

- any new attributes you create in the schema must use one of the predefined syntaxes.
- when create a new attribute, need specify 1. OM syntax, 2. the OID of the syntax
- MSFT added 22 expanded syntaxes: [Syntax definitions](https://learning.oreilly.com/library/view/active-directory-5th/9781449361211/ch05.html#syntax_definitions)

`systemflags`

- a bit mask that represents how the attribute should be handled.
- can be configured on both schema and object
- flag (0x02000000): The object is not moved to the Deleted Objects container. The tombstone remains in the original container.
- Constructed attributes are not stored. e.g., `msDS-Approx-Immed-Subordinates` tells how many objects under a container.
- Category 1 objects: a subset of the attributes and classes that come with AD LDS or Active Directory.

`schemaFlagsEx`

- hold flags that further define the properties of an attribute.

`searchFlags`

- control indexing
- only set on schema attribute definitions
- [Search Flag values](https://learning.oreilly.com/library/view/Active+Directory,+5th+Edition/9781449361211/ch05.html#search_flag_bits)
- (0x0001): enable creating index
- (0x0002): create index of the attr within each container
- (0x0004): add the attr to ambiguous name resolution (ANR) set
- bitwise query, NOT query will still enumerate all objects even the attr is indexed
- Linked attributes, such as group membership, are implicitly indexed.
- multi-value attr or non-uniq attr can also be indexed
- `objectClass` is indexed by default
- indexes consume disk space of the DB file `ntds.dit`. domain controller performance will also be impacted. Index data is not replicated
- can enable deferred indexing. Need to update forest's `dsHeuristics` first.
- Can set attr to be remained on tombstoned object. The attr doesn't need restore when the object is restored. Need first enable Active Directory Recycle Bin. Linked attr cannot be perserved
- The subtree index: allows virtual list view (VLV)
- The tuple index: normal index supports direct lookups and trailing wildcard. This index allow wildcards anywhere.
- Confidentiality: attr needs two permissions in order to be viewed by a trustee
- access control entries (ACEs)
- Attribute change auditing
- The filtered attribute set: new read-only domain controller (RODC) functionality

Property sets and attributeSecurityGuid

- property sets are defined in the `cn=extended-rights` subcontainer as `controlAccessRight` objects.

linked attributes

- consist of a forward link and a back link, e.g., member and memberOf.
- Attributes are linked by setting the `linkID` attributes of two `attributeSchema` objects to valid link values.

MAPI IDS

- attr displays in Exchange global address list (GAL) must have a MAPI ID

#### 5.4. Classes (classSchema Objects)

[All attr on classSchema](https://docs.microsoft.com/en-us/windows/win32/adschema/c-classschema?redirectedfrom=MSDN)

Object class category and inheritance

- attribute `subClassOf`
- `top`: base class of all objects
- objectClassCategory
  1. Structural: directly create objects of its type
  2. Abstract: can inherit from other classes and can have attributes defined on them. can inherit only from another abstract class.
  3. Auxiliary: store sets of attributes that other classes can inherit. cannot inherit from a structural class.
  4. 88-Class: deprecated category

Dissecting an example active directory class

- atrribute `auxiliaryClass`, it is a string lists auxiliary classes on this class.
- attribute `objectClass` shows what the inherit chain looks like of this class. All class schema classes are inherit from `top` and `classSchema`.
- `rDNAttID`: defines what prefix to use when connecting to instances of the class via LDAP. Normally it should be `cn`.
- `systemPossSuperiors`: what parents can the class to be created under.
- `mustContain`, `mayContain`, `possSuperiors`, and `auxiliaryClass`: inheritance affects them.

Dynamically linked auxiliary classes

- can dynamically assign auxiliary classes to individual objects without update the class schema of the objects

### Chapter 6. Site Topology and Active Directory Replication

multimaster replication: need create a site topology that describes the network and helps define how domain controllers should replicate with each other. Complicate topology require a significant amount of work.

single-master replication scheme problems

- the single point of failure for updates
- geographic distance from the master to clients performing the updates
- less efficient replication due to updates having a single originating location

Knowledge Consistency Checker (KCC)

#### 6.1. Site Topology

major components: sites, subnets, site links, site link bridges, and connection objects.

Site and replication management tools

- Sites container: holds all the site topology objects and connection objects. directly under the Configuration container

Subnets

- a portion of the IP space of a network
- determining relative locations of the machines.
- associated with sites. determine what distributed resources it should try to use.
- Supernet: a single subnet that encompasses one or more smaller subnets. used as a "catchall" subnet.

Sites

- group subnets together into logical collections to help define replication flow and resource location boundaries.
- The client’s IP address is used to determine which Active Directory subnet the client belongs to, and then look up the AD site.
- There is a Default-First-Site-Name site.

Site links

- what sites are connected to each other and the relative cost of the connection.
- can come in two replication flavors: IP and SMTP.
  - IP: represents a remote procedure calls (RPC)-style replication connection.
  - Simple Mail Transfer Protocol (SMTP): use when have very poor or unreliable network connections
- Active Directory will automatically register the necessary covering DNS records for sites that do not have the target domain controllers, based on the site link topology.

Site link bridges

- by default the network paths between all of the sites are transitive
- when network is not being fully routed, need to define site link bridges
- A site link bridge contains a set of site links that can be considered transitive.

Connection objects

- specifies which domain controllers replicate with which other domain controllers, how often, and which naming contexts are involved.
- Before, need to use Active Directory Load Balancing (ADLB) tool
- Now, automatically load balance replication connection to read-only domain controllers (RODCs).
- Define connection objects to override.

Knowledge consistency checker (KCC)

- site topology: maps closely to the physical network
- after set up sites, subnets, and site link objects, KCC generates connection objects
- two separate algorithms: intrasite and intersite.
  - intrasite: create a minimal latency ring topology for each naming context. guarantees no more than three hops between any two domain controllers in a site
  - intersite: keep the sites connected via a spanning-tree algorithm so that replication can occur, and then simply follows the site link metrics for making those connections.

#### 6.2. How Replication Works

A background to metadata

- Replication metadata: enables data transfer between naming contexts on different domain controllers
- Authentication between domain controllers for replication is Kerberos-based, and Kerberos has a maximum time-skew requirement between hosts.
- Update sequence numbers (USNs) and highestCommittedUSN: each domain controller maintains its own USN. Only meaningful on the domain controller perform the update. each domain controller stores `highestCommittedUSN` value of the `RootDSE`.
- Originating updates versus replicated updates: both increase the same amount of times
- Directory Service Agent (DSA) GUID: used to uniquely identify a domain controller.
- invocation ID is used to identify the server’s Active Directory database in replication. is changed any time Active Directory is restored on that DC or any time the DSA GUID is changed.
- High-watermark vector (HWMV/direct up-to-dateness vector): a table maintained independently by every domain controller to assist in efficient replication of a naming context, stores the highest USN of the updates the domain controller has received from each direct partner it replicates with for the given naming context.
- Up-to-dateness vector: used for replication dampening to reduce needless replication traffic and endless replication loops. so called propagation dampening.

How an object’s metadata is modified during replication

- The originating DC USN records the USN for a change on the target DC.
- The same USN is replicated to other DCs
- The USN is recorded on the changed attribute.
- Attribute also has a version number and a timestamp
- those data are not attributes, but metadata. All stored in `replPropertyMetaData`

The replication of a naming context between two servers

- Not necessary to replica between each two servers, because one server can replicate to another server and that server can replicate to more servers
- Replication is a five-step process:
  1. Replication with a partner is initiated. The init server wants the updates, while the partner sends the updates.
  2. The partner works out what updates to send. Comparing USN with HWMV. The partner also keeps a copy of UTDV from init server.
  3. The partner sends the updates to the initiating server.
  4. The initiating server processes the updates.
  5. The initiating server checks whether it is up to date.

How replication conflicts are reconciled

- Conflict due to identical attribute change: attr has higher version number + earlier timestamp + higher server Guid wins the conflict
- Conflict due to a move or creation of an object under a now-deleted parent: move the obj to Lost and Found container.
- Conflict due to creation of objects with names that conflict: similar to the id conflict, but after find out the winner, change the name of the other one.
- Replicating the conflict resolution: two servers first solve the conflict, then replicate to others

6.3. Common Replication Problems

Lingering objects

- happens when a domain controller that has been offline for longer than the tombstone lifetime
- demote the domain controller and then re-promote it can solve the issue

USN rollback

- rolling back virtual machine snapshots and the use of image-based disk backup programs can cause the issue
- Before making changes to the restored machine, first demote it and re-promote it.

### Chapter 7. Searching Active Directory

#### 7.1. The Directory Information Tree

directory information tree (DIT): Database on each domain controller stored in the ntds.dit file.

Database structure

- key tables
  - data table: all the data regardless of its class. The Extensible Storage Engine (ESE) is very efficient at storing null values so leave attr as null value is fine.
    - Each row represents an object and has an identifier distinguished name tag (DNT). It keeps increasing in the same domain controller but not replicate to other controllers.
    - PDNT: the parent's DNT of this object.
    - NCDNT: contains the DNT of the naming context head matching the NC in which the object resides.
    - Ancestors: listing of all of the DNTs between the root of the database and that object
    - RDNType: stores the DNT of the RDN attribute in the schema for the object
  - link table: store links
  - hidden table: to find configuration-related information, i.e., NTDS Settings object.
- link tables
  - store linked attr like group membership
  - stores link DNT and the backlink DNT. LinkBase is the link attribute.
- Security descriptor table
  - stores access control lists (ACLs)

#### 7.2. Searching the Database

Query processor transform LDAP filter into commands to retrieve data from DIT.

Filter operators

- Return all the objects have an attr of a special value: `attrname=value`
- Filter operators
  - `=`
  - `<=`
  - `>=`
  - `!`: `(!(department=Accounting))`, so find all the objects where the `department` attr is not `Accounting`
- `*`: any value. `(!(description=*))`, all objects that don't have `description` attr. Support wild card as well.

Connecting filter components

- Boolean operators
  - `&`
  - `|`
- `(&(objectClass=user)(department=Accounting))`

Search bases

- Subtree search: `adfind -f "(objectClass=user)" -b "OU=People,DC=cohovines,DC=com"`
- One-level search: add `-s onelevel`
- base-level search

Modifying behavior with ldap controls

- session options that are sent as part of the query via the LDAP protocol
- `LDAP_PAGED_RESULT_OID_STRING`: page size
- `LDAP_SERVER_DIRSYNC_OID`: find all objs that have been changed since last search. Changes are tracked with a cookie that is returned by the server.
- `LDAP_SERVER_RANGE_OPTION_OID`: retrieve values in a multivalued attribute in excess of the maximum number of values
- `LDAP_SERVER_SEARCH_OPTIONS_OID`: Search across all domains. phantom root: perform a search across all of the NCs hosted on a global catalog.
- `LDAP_SERVER_SHOW_DELETED_OID`, `LDAP_SERVER_SHOW_RECYCLED_OID`, `LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID`: include deleted objects and tombstones in the result set.
- `LDAP_SERVER_SORT_OID`: if the attr is not indexed, AD will use a temporary table
- `LDAP_CONTROL_VLVREQUEST`: Virtual list view (VLV) searches are useful for large searches that will be paged through
- `LDAP_SERVER_ASQ_OID`: Attribute scoped queries (ASQs) are useful when you want to perform a query based on a linked attribute’s value(s).

#### 7.3. Attribute Data Types

Dates and times

- NT FILETIME: an 64-bit int. In powershell, uses `[DateTime]::Parse("10/01/2012")` to convert.
- Format: `YYMMDDHHMMSS.fffZ`

Bit masks

- `(userAccountControl:AND:=2)`: Find when the 2nd bit of the `userAccountControl` is set.

The in-chain matching rule

- walk a group’s membership chain forward or backward
- `(memberof:INCHAIN:=(cn=Important People,OU=Groups,DC=cohovines,DC=com))`
- `(member:INCHAIN:=(cn=Brian,OU=People,DC=cohovines,DC=com))`

#### 7.4. Optimizing Searches

Efficient searching

- Build index
- Using the stats control: show Elapsed Time, Returned X entries of Y visited, Used Indices, Pages Referenced, etc.

objectClass versus objectCategory

- `objectClass` was not indexed so add `objectCategory` to index
- now it is not the case

### Chapter 8. Active Directory and DNS

#### 8.1. DNS Fundamentals

DNS is a hierarchical name-resolution system: hostname to IP address, IP address to hostname, and hostname to alternate hostname (aliases)

It is the largest public directory service deployed.

3 most important DNS concepts

- Zones are delegated portions of the DNS namespace.
- Resource records contain name-resolution information.
- Dynamic DNS allows clients to add and delete resource records dynamically.

Zones

- a collection of hierarchical domain names
- root has been delegated to name servers: mycorp.com namespace was delegated to the name server ns1.mycorp.com. mycorp.com is a zone.
- subdomain1.mycorp.com could be delegated to another name server ns2.mycorp.com. subdomain1.mycorp.com become a new zone.

Resource records

- the unit of information in DNS.
- A zone is essentially a collection of resource records.
- types
  - Address record: `A` for IPv4 and `AAAA` for IPv6.
  - Alias record: `CNAME`
  - Name server record: `NS`
  - Service record: `SRV`, Maps a particular service (e.g., LDAP) to hostnames and ports. It points to a fully qualified names of the A records as well.

Client lookup process

- A caching DNS server is used. The caching then reaches to root, .com and lower level name servers
- the linkage between root and .com name server is called a delegation, which contains NS records

Dynamic DNS

- client can send a DNS server requests to add/delete resource records in a zone
- Need have ACL in place

Global names zones

- can configure to support short name resolution via DNS without the DNS-suffix search-list requirements on clients

#### 8.2. DNSSEC

In between the client machine and the authoritative DNS server hosting a given zone, there are often one or more nonauthoritative caching DNS servers involved.

How does DNSSEC work?

- a trust chain that begins with the root DNS servers
- the publication of digital signatures in DNS for individual records and the zone as a whole.
- the ISP need to support DNSSEC on its DNS servers, then the client can can enable DNSSEC.
- Powershell command `Resolve-DnsName` can used to check if DNSSEC is enabled

Configuring DNSSEC for active directory DNS

#### 8.3. DC Locator

How a client can find the most optimal domain controller (DC) to authenticate against: use DNS to locate domain controllers via the DC locator process

#### 8.4. Resource Records Used by Active Directory

%SystemRoot%\System32\Config\netlogon.dns: contains the necessary resource records for a domain controller.

Overriding SRV record registration

- do not want domain controllers or global catalogs publishing some or all of their records outside of the site they are in

#### 8.5. Delegation Options

Set up who is authoritative for the Active Directory-related zones

Not delegating the AD DNS zones

- Not good if think about 1. Political factors, 2. Initial setup and configuration, 3. Support and maintenance, 4. Integration issues

Delegating the AD DNS zones

#### 8.6. Active Directory-Integrated DNS

Host AD DNS zones on domain controllers

Conditional forwarding: set up one or more IP addresses to forward all requests

Replication impact

Background zone loading

#### 8.7. Using Application Partitions for DNS

Store options

- `cn=System,<DomainDN>`
- `dc=DomainDnsZones, <DomainDN>`
- `dc=ForestDnsZones, <ForestDN>`
- `AppPartitionDN`

#### 8.8. Aging and Scavenging

Scavenging is a background process that you configure on a per-DNS-server basis to scan all of the records in a zone and remove the records that have not been refreshed in a certain time period.

Configuring scavenging

- Setting zone-specific options
- Enabling scavenging on the DNS server

#### 8.9. Managing DNS with Windows PowerShell

```powershell
Import-Module DnsServer
Get-Command -Module DnsServer
```

### Chapter 9. Domain Controllers

read-only domain controller (RODC): Ensuring the physical security of Active Directory domain controllers

#### 9.1. Building Domain Controllers

Install from Media (IFM)

promotion: converting a member server to a domain controller.

Deploying with server manager

- install the Active Directory binaries
- Promote this server to a domain controller

Using dcpromo on earlier versions of windows

Automating the DC build process

- `Add-WindowsFeature -Name AD-Domain-Services -IncludeManagementTools`
- `Install-ADDSDomainController`

#### 9.2. Virtualization

relative identifier (RID)

USN rollback

When to virtualize

- security issue: anyone who has access to the hypervisor storage where the virtual hard disks (VHDs) are stored effectively has physical access to the domain controller.
- antiaffinity: level of redundancy. Where are the virtual machine locates such that they are hosted across multiple physical hosts and data storage devices

Impact of virtualization

- USN rollback
- RID pool reuse or duplication: RIDs are unique numbers that are used to construct a unique SID for each security principal
- System clock changes

Virtualization safe restore

- virtual machine generation identifier (VM Gen ID)

Cloning domain controllers

- need to create a cloning configuration file
- a cloning allow list
- The DC cloning process: after completed the prerequisite steps, duplicated the virtual hard disk file, created the configuration files, and started the cloned virtual machine for the first time, Active Directory’s cloning process will begin
- Directory Services Restore Mode (DSRM)
- Cloning a domain controller

#### 9.3. Read-Only Domain Controllers

**HERE**: <https://learning.oreilly.com/library/view/active-directory-5th/9781449361211/ch09.html>
