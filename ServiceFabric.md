# Service Fabric

## Microservices

[wiki](https://en.wikipedia.org/wiki/Microservices)

- SOA
- services should be fine-grained and the protocols should be lightweight.
- decomposing an application into different smaller services.

## Azure Service Fabric

[doc](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-overview)

### Quick Start

[ARM Template](https://docs.microsoft.com/en-us/azure/service-fabric/quickstart-cluster-template)

- Used to create resources

[Managed Cluster](https://docs.microsoft.com/en-us/azure/service-fabric/quickstart-managed-cluster-template)

- an evolution of the Azure Service Fabric cluster resource model

[Create .NET application](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-quickstart-dotnet)

- Visual studio install `Azure development` and `ASP.NET and web development`
- Install the [Microsoft Azure Service Fabric SDK](https://www.microsoft.com/web/handlers/webpi.ashx?command=getinstallerredirect&appid=MicrosoftAzure-ServiceFabric-CoreSDK)
- Deploy local cluster: `Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force -Scope CurrentUser`
- Follow [Prep Windows for containers](https://docs.microsoft.com/en-us/virtualization/windowscontainers/quick-start/set-up-environment?tabs=Windows-10) to set up docker if wants to use container
- Create a dev cluster: `C:\Program Files\Microsoft SDKs\Service Fabric\ClusterSetup\DevClusterSetup.ps1`
- `Connect-ServiceFabricCluster` to connect to it.
- Start Service Fabric Manager: `C:\Program Files\Microsoft SDKs\Service Fabric\Tools\ServiceFabricLocalClusterManager\ServiceFabricLocalClusterManager.exe`
- [Code example](https://github.com/Azure-Samples/service-fabric-dotnet-quickstart)
- VotingWeb is an stateless app serves as the front end. It is also a service fabric app.
- VotingData is a stateful app serves as the back end. The data is stoled in the reliable dict so no database is needed.
- The VotingWeb is a MVC app. It uses ServiceRuntime to run as an service fabric app.

### Concepts

[Understand microservices](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-overview-microservices)

- back-end store, middle-tier business logic, and a front-end user interface (UI).
- old way monolithic application: The interfaces tended to be between the tiers, and a more tightly coupled design was used between components within each tier. Calls are over interprocess communication (IPC)
- Microservices: each one service typically encapsulates simpler business functionality, which you can scale out or in, test, deploy, and manage independently.

[Big Picture](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-content-roadmap)

- service type: name/version assigned to a service's code packages. Defined in `ServiceManifest.xml`. e.g.: `VotingWebType`, `VotingDataType`
- service package and manifest: disk directory containing the service type's ServiceManifest.xml file, which references the code, static data, and configuration packages for the service type.
- application type: name/version assigned to a collection of service types. Defined in `ApplicationManifest.xml`. e.g.: `VotingType`.
- application package and manifest.
- The files in the application package directory are copied to the Service Fabric cluster's image store.
- can then create a named application within the cluster
- After creating a named application, you can create a named service from one of the application type's service types.

- Service Fabric cluster: a network-connected set of virtual or physical machines
- Node: A machine or VM that is part of a cluster. Each node runs `FabricHost.exe` which starts `Fabric.exe` and `FabricGateway.exe`
- Application instance: a version of the application. Each application type instance is assigned a URI name that looks like fabric:/MyNamedApp.
- Service instance url looks like: `fabric:/MyNamedApp/MyDatabase`
- Stateless services can store persistent state in an external storage
- A stateful service stores state within the service and uses Reliable Collections or Reliable Actors programming models to manage state.
- partition scheme: Each partition is responsible for a portion of the complete state of the service, which is spread across the cluster's nodes. Within a partition there could be multiple replicas (for stateful) or instances (for stateless)
- Replicas: Read and write operations are performed at one replica (called the Primary). Changes to state from write operations are replicated to multiple other replicas (called Active Secondaries).

- online transaction processing (OLTP) service: high-throughput, low-latency, failure-tolerant. keeping code and data close on the same machine.

[Here](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-content-roadmap#supported-programming-models)

## Other Notes

[Volumes](https://docs.microsoft.com/en-us/azure/service-fabric-mesh/service-fabric-mesh-storing-state#volumes)

- Volumes are directories that get mounted inside your container instances that you can use to persist state.

[Naming Service](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-communication)

- The Naming Service is a registrar for services and their addresses that each instance or replica of the service is listening on.

[Management Client](https://docs.microsoft.com/en-us/python/api/azure-mgmt-servicefabric/azure.mgmt.servicefabric.ServiceFabricManagementClient?view=azure-python)

- `ServiceFabricManagementClient` can be used to connect to cluster.

[Service Fabric temp folder](https://docs.microsoft.com/en-us/dotnet/api/system.fabric.codepackageactivationcontext.tempdirectory?view=azure-dotnet)

- A temp folder that can be used for SF. But how the lifecycle does it have?

[SF work folder](https://social.msdn.microsoft.com/Forums/sqlserver/en-US/3d7b0d30-b084-4253-9a3e-49f9f1b9c1b2/how-do-i-get-files-into-the-work-directory-of-a-stateless-service?forum=AzureServiceFabric)

To access, in `RunAsync()`:

```C#
string tempFolder = this.Context.CodePackageActivationContext.TempDirectory;
string workFolder = this.Context.CodePackageActivationContext.WorkDirectory;
```

[SF App manifast](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-application-and-service-manifests)

- A service manifest can contain multiple code, configuration, and data packages, which can be versioned independently.
- Config package is the contents of the Config directory under PackageRoot that contains an independently-updateable and versioned set of custom configuration settings for your service.

<https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-application-lifecycle>

<https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-how-to-parameterize-configuration-files>

<https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-application-upgrade-parameters>

<https://stackoverflow.com/questions/33928204/where-do-you-set-and-access-run-time-configuration-parameters-per-environment-fo/45998366>

<https://stackoverflow.com/questions/58932068/how-do-i-update-service-fabric-application-parameter-using-powershell>
