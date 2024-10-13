using Microsoft.Extensions.Configuration;

namespace AzureManagementTest
{
    internal class Configuration
    {
        internal const string ConfigFile = "appsettings.json";
        internal const string TenantIdKey = "TenantId";
        internal const string ResourceGroupNameKey = "ResourceGroupName";
        internal const string KeyVaultNameKey = "KeyVaultName";

        internal static T GetConfigFromFile<T>(IConfiguration configSource, string configKey)
        {
            T? configValue = configSource.GetSection("Settings").GetValue<T>(configKey);
            if (configValue == null)
            {
                throw new ApplicationException($"{configKey} not found in {ConfigFile}!");
            }

            return configValue;
        }
    }
}
