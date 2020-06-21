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
- Schema Partition
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

**HERE**: <https://learning.oreilly.com/library/view/active-directory-5th/9781449361211/ch06.html>
