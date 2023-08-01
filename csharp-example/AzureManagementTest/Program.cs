using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Compute;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// Follow https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/README.md#authenticate-via-visual-studio-code
// Use Azure Account extension to sign in.
ArmClient client = new ArmClient(new DefaultAzureCredential());
SubscriptionResource subscription = client.GetDefaultSubscription();

var resourceGroup = subscription.GetResourceGroup("WinServer2022VM");

// Below code written by chatgpt
// // Create the resource group
// var resourceGroup = new ResourceGroup
// {
//     Location = location
// };

// // VirtualMachineCollection vms
// string subscriptionId = "YOUR_SUBSCRIPTION_ID";
// string clientId = "YOUR_CLIENT_ID";
// string clientSecret = "YOUR_CLIENT_SECRET";
// string tenantId = "YOUR_TENANT_ID";
// string resourceGroupName = "YOUR_RESOURCE_GROUP_NAME";
// string vmName = "YOUR_VM_NAME";
// string location = "YOUR_VM_LOCATION"; // e.g., "eastus"

// // Authenticate using a service principal
// var serviceClientCredentials = ApplicationTokenProvider.LoginSilentAsync(tenantId, clientId, clientSecret).Result;

// // Create a Resource Management client
// var resourceManagementClient = new ResourceManagementClient(serviceClientCredentials)
// {
//     SubscriptionId = subscriptionId
// };

// resourceManagementClient.ResourceGroups.CreateOrUpdate(resourceGroupName, resourceGroup);

// // Create the VM
// var vmClient = new ComputeManagementClient(serviceClientCredentials)
// {
//     SubscriptionId = subscriptionId
// };

// var vmParams = new VirtualMachine
// {
//     Location = location,
//     OsProfile = new OSProfile
//     {
//         ComputerName = vmName,
//         AdminUsername = "YOUR_ADMIN_USERNAME", // Replace with your desired admin username
//         AdminPassword = "YOUR_ADMIN_PASSWORD"  // Replace with your desired admin password
//     },
//     HardwareProfile = new HardwareProfile
//     {
//         VmSize = "Standard_D2s_v3" // Replace with the desired VM size
//     },
//     StorageProfile = new StorageProfile
//     {
//         ImageReference = new ImageReference
//         {
//             Publisher = "MicrosoftWindowsServer",
//             Offer = "WindowsServer",
//             Sku = "2022-datacenter",
//             Version = "latest"
//         },
//         OsDisk = new OSDisk
//         {
//             CreateOption = DiskCreateOptionTypes.FromImage,
//             ManagedDisk = new ManagedDiskParameters
//             {
//                 StorageAccountType = StorageAccountTypes.StandardLRS
//             }
//         }
//     },
//     NetworkProfile = new NetworkProfile
//     {
//         NetworkInterfaces = new[]
//         {
//                         new NetworkInterfaceReference
//                         {
//                             Id = "/subscriptions/YOUR_SUBSCRIPTION_ID/resourceGroups/YOUR_RESOURCE_GROUP/providers/Microsoft.Network/networkInterfaces/YOUR_NETWORK_INTERFACE_NAME"
//                             // Replace with the actual network interface ID
//                         }
//                     }
//     }
// };

// vmClient.VirtualMachines.CreateOrUpdate(resourceGroupName, vmName, vmParams);

// Console.WriteLine("Virtual Machine created successfully!");
