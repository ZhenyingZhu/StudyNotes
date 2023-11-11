namespace AzureManagementTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure;
    using Azure.Core;
    using Azure.Identity;
    using Azure.ResourceManager;
    using Azure.ResourceManager.Resources;
    using Azure.Security.KeyVault.Certificates;
    using Azure.Security.KeyVault.Keys;
    using Azure.Security.KeyVault.Secrets;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    internal class AzureClient
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        internal ArmClient? ArmClient { get; private set; }
        internal SubscriptionResource? Subscription { get; private set; }
        internal ResourceGroupResource? ResourceGroup { get; private set; }
        internal CertificateClient? CertClient { get; private set; }
        internal KeyClient? KeyClient { get; private set; }
        internal SecretClient? SecretClient { get; private set; }

        public AzureClient(ILogger<MyHostedService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            // https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/README.md
            var vsCreds = new VisualStudioCredential(new VisualStudioCredentialOptions()
            {
                TenantId = Configuration.GetConfigFromFile<string>(_config, Configuration.TenantIdKey)
            });
            ArmClient = new ArmClient(vsCreds);

            string keyVaultName = Configuration.GetConfigFromFile<string>(_config, Configuration.KeyVaultNameKey);
            string keyVaultEndpoint = $"https://{keyVaultName}.vault.azure.net/";
            Uri keyVaultUri = new Uri(keyVaultEndpoint);
            CertClient = new CertificateClient(keyVaultUri, vsCreds);
            KeyClient = new KeyClient(keyVaultUri, vsCreds);
            SecretClient = new SecretClient(keyVaultUri, vsCreds);

            Subscription = ArmClient.GetDefaultSubscription();

            _logger.LogInformation($"Use the subscription {Subscription.Id}.");

            // https://learn.microsoft.com/en-us/dotnet/api/overview/azure/resourcemanager-readme?view=azure-dotnet
            string resourceGroupName = Configuration.GetConfigFromFile<string>(_config, Configuration.ResourceGroupNameKey);
            ResourceGroupCollection groups = Subscription.GetResourceGroups();
            var response = groups.GetIfExists(resourceGroupName);
            if (!response.HasValue)
            {
                ResourceGroupData groupData = new ResourceGroupData(AzureLocation.WestUS);
                ResourceGroup = groups.CreateOrUpdate(WaitUntil.Completed, resourceGroupName, groupData).Value;
            }
            else
            {
                ResourceGroup = response.Value;
            }

            _logger.LogInformation($"Use the resource group {resourceGroupName}.");
        }
    }
}
