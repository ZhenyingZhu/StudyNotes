using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


string message = null;

try
{
    SecretClientOptions options = new SecretClientOptions()
    {
        Retry =
        {
            Delay= TimeSpan.FromSeconds(2),
            MaxDelay = TimeSpan.FromSeconds(16),
            MaxRetries = 5,
            Mode = RetryMode.Exponential
         }
    };

    SecretClient client = new SecretClient(
        new Uri("https://ZhenyingKeyVault.vault.azure.net/"),
        new ManagedIdentityCredential(),
        options);

    KeyVaultSecret secret = client.GetSecret("TestSecret");

    string secretValue = secret.Value;

    message = $"Get secret: {secretValue}";
}
catch (Exception ex)
{
    message = ex.ToString();
}

app.MapGet("/", () => "Hello World! " + message);

app.Run();
