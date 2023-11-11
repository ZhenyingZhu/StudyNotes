using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AzureManagementTest;
using Microsoft.Extensions.Configuration;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Program start.");

// DI also read from appsettings.json: https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<AzureClient>();
builder.Services.AddHostedService<MyHostedService>();

IHost host = builder.Build();

// Directly read config files: https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration
IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
//int? config1 = config.GetValue<int>(Constants.TenantIdKey);
string? tenantId = config.GetSection("Settings").GetValue<string>(Configuration.TenantIdKey);

host.Run();

Console.WriteLine("Program end.");