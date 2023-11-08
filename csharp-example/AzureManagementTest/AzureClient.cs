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

        public AzureClient(ILogger<MyHostedService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            string? tenantId = _config.GetSection("Settings").GetValue<string>(Constants.TenantIdKey);
            if (tenantId == null)
            {
                throw new ApplicationException($"{Constants.TenantIdKey} not found in {Constants.ConfigFile}!");
            }

            // https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/README.md
            ArmClient = new ArmClient(new VisualStudioCredential(new VisualStudioCredentialOptions()
            {
                TenantId = tenantId
            }));

            Subscription = ArmClient.GetDefaultSubscription();

            _logger.LogInformation($"Use the subscription {tenantId}.");

            // https://learn.microsoft.com/en-us/dotnet/api/overview/azure/resourcemanager-readme?view=azure-dotnet
            string? resourceGroupName = _config.GetSection("Settings").GetValue<string>(Constants.ResourceGroupNameKey);
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

            string keyVaultEndpoint = "";
            Uri keyVaultUri = new Uri(keyVaultEndpoint);
            CertClient = new CertificateClient(keyVaultUri ,new VisualStudioCredential(new VisualStudioCredentialOptions()
            {
                TenantId = tenantId
            }));
        }
    }
}
