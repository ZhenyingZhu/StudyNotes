using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Compute;
using AzureManagementTest;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// Follow https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/README.md#authenticate-via-visual-studio-code
// Use Azure Account extension to sign in.
// Seems like need to set env vars.
ArmClient client = new ArmClient(new VisualStudioCredential(new VisualStudioCredentialOptions()
{
    TenantId = "c74a24d8-2986-4739-aec1-36b4c9934ed3"
}));

SubscriptionResource subscription = client.GetDefaultSubscription();

VMManagement vmClient = new VMManagement(client);

var resourceGroup = subscription.GetResourceGroup("WinServer2022VM");

// Below code written by chatgpt
// // Create the resource group
// var resourceGroup = new ResourceGroup
// {
//     Location = location
// };
