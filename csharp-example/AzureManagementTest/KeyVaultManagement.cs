namespace AzureManagementTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class KeyVaultManagement
    {
        AzureClient _azureClient;

        public KeyVaultManagement(AzureClient azClient)
        {
            // https://learn.microsoft.com/en-us/azure/key-vault/general/network-security?WT.mc_id=Portal-Microsoft_Azure_KeyVault
            // https://learn.microsoft.com/en-us/azure/private-link/private-endpoint-overview
            _azureClient = azClient;
        }
    }
}
