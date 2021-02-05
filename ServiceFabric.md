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
- VotingData is a stateful app serves as the back end.
