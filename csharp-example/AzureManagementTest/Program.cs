using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AzureManagementTest;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Program start.");

// Directly read config files: https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration
// DI also read from appsettings.json: https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<MyHostedService>();

IHost host = builder.Build();
host.Run();

Console.WriteLine("Program end.");