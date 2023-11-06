namespace AzureManagementTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Azure;
    using Azure.Core;
    using Azure.Identity;
    using Azure.ResourceManager;
    using Azure.ResourceManager.Resources;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    internal class AzureClient
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        internal ArmClient ArmClient { get; }
        internal SubscriptionResource Subscription { get; }
        internal ResourceGroupResource ResourceGroup { get; }

        public AzureClient(ILogger<MyHostedService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            string? tenantId = _config.GetValue<string>(Constants.TenantIdKey);
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

            // https://learn.microsoft.com/en-us/dotnet/api/overview/azure/resourcemanager-readme?view=azure-dotnet
            string? resourceGroupName = _config.GetValue<string>(Constants.ResourceGroupNameKey);
            ResourceGroupResource resourceGroup = Subscription.GetResourceGroup(resourceGroupName);
            if (resourceGroup == null)
            {
                ResourceGroupData groupData = new ResourceGroupData(AzureLocation.WestUS);

                ResourceGroupCollection groups = Subscription.GetResourceGroups();
                ResourceGroup = groups.CreateOrUpdate(WaitUntil.Completed, resourceGroupName, groupData).Value;
            }
            else
            {
                ResourceGroup = resourceGroup;
            }
        }
    }
}
