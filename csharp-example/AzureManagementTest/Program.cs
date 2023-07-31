// See https://aka.ms/new-console-template for more information
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Compute;

Console.WriteLine("Hello, World!");

ArmClient client = new ArmClient(new DefaultAzureCredential());

SubscriptionResource subscription = client.GetDefaultSubscription();

// VirtualMachineCollection vms

ResourceGroupResource resourceGroup = subscription.GetResourceGroup("WinServer2022VM");
