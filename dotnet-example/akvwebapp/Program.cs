using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using Microsoft.Extensions.Logging.AzureAppServices;

// https://learn.microsoft.com/en-us/azure/key-vault/general/tutorial-net-create-vault-azure-web-app?tabs=azure-cli
var builder = WebApplication.CreateBuilder(args);

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-8.0
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// https://stackoverflow.com/questions/74503796/should-i-use-for-net-logging-in-azure-addazurewebappdiagnostics-or-addapplicat
builder.Logging.AddAzureWebAppDiagnostics();

builder.Services.Configure<AzureFileLoggerOptions>(options =>
{
    options.FileName = "Diagnostics-Logs";
    options.FileSizeLimit = 50 * 1024;
    options.RetainedFileCountLimit = 3;
});

var app = builder.Build();

app.Logger.LogInformation("App starts.");

string message = string.Empty;

try
{
    SecretClientOptions options = new SecretClientOptions()
    {
        Retry =
        {
            Delay= TimeSpan.FromSeconds(1), // 2
            MaxDelay = TimeSpan.FromSeconds(5), // 16
            MaxRetries = 2, // 5,
            Mode = RetryMode.Exponential
         }
    };

    SecretClient client = new SecretClient(
        new Uri("https://ZhenyingKeyVault.vault.azure.net/"),
        // new ManagedIdentityCredential(),
        new DefaultAzureCredential(),
        options);

    app.Logger.LogInformation("AKV Client created.");

    KeyVaultSecret secret = client.GetSecret("TestSecret");

    string secretValue = secret.Value;

    message = $"Get secret: {secretValue}";

    app.Logger.LogInformation($"Secret fetched: {secretValue}");
}
catch (Exception ex)
{
    message = ex.ToString();
    app.Logger.LogError(ex, "Error occurred.");
}

app.MapGet("/", () => "Hello World! " + message);

app.Run();
