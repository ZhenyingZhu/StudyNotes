namespace AzureManagementTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.ResourceManager.Compute.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    internal sealed class MyHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly AzureClient _client;

        public MyHostedService(ILogger<MyHostedService> logger, IConfiguration config, AzureClient azureClient)
        {
            _logger = logger;
            _config = config;
            _client = azureClient;

            _logger.LogInformation($"Resource Group Name: {_client.ResourceGroup?.Data.Name}");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{this.GetType().Name} starts...");

            var sec = _client.SecretClient?.GetSecret("TestSecret").Value;
            _logger.LogInformation(sec?.ToString());

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{this.GetType().Name} ends...");

            return Task.CompletedTask;
        }
    }
}
