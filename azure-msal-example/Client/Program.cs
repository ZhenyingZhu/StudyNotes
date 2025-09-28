using Microsoft.Identity.Client;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Client;

class Program
{
    private static IPublicClientApplication? _app;
    private static string[]? _scopes;
    private static string? _apiBaseUrl;
    private static HttpClient _httpClient = new HttpClient();

    static async Task Main(string[] args)
    {
        Console.WriteLine("Azure MSAL Client Example");
        Console.WriteLine("========================");

        // Load configuration
        var configuration = LoadConfiguration();
        if (!InitializeApp(configuration))
        {
            Console.WriteLine("Failed to initialize the application. Please check your configuration.");
            return;
        }

        try
        {
            // Authenticate the user
            var accessToken = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("Failed to acquire access token.");
                return;
            }

            Console.WriteLine("Successfully authenticated!");
            Console.WriteLine();

            // Interactive menu
            while (true)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Send a message to the server");
                Console.WriteLine("2. Get protected data");
                Console.WriteLine("3. Exit");
                Console.Write("Choice: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await SendMessageAsync(accessToken);
                        break;
                    case "2":
                        await GetProtectedDataAsync(accessToken);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    private static bool InitializeApp(IConfiguration configuration)
    {
        try
        {
            var clientId = configuration["AzureAd:ClientId"];
            var authority = $"{configuration["AzureAd:Instance"]}{configuration["AzureAd:TenantId"]}";
            var redirectUri = configuration["AzureAd:RedirectUri"];
            _apiBaseUrl = configuration["Api:BaseUrl"];
            _scopes = configuration.GetSection("Api:Scopes").Get<string[]>();

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(authority) || 
                string.IsNullOrEmpty(_apiBaseUrl) || _scopes == null)
            {
                return false;
            }

            _app = PublicClientApplicationBuilder
                .Create(clientId)
                .WithAuthority(new Uri(authority))
                .WithRedirectUri(redirectUri)
                .WithDefaultRedirectUri()
                .Build();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Configuration error: {ex.Message}");
            return false;
        }
    }

    private static async Task<string?> GetAccessTokenAsync()
    {
        try
        {
            // Try to get token silently first
            var accounts = await _app!.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();

            AuthenticationResult result;
            if (firstAccount != null)
            {
                try
                {
                    result = await _app.AcquireTokenSilent(_scopes, firstAccount)
                        .ExecuteAsync();
                    Console.WriteLine("Token acquired silently.");
                }
                catch (MsalUiRequiredException)
                {
                    // Silent token acquisition failed, fall back to interactive
                    result = await _app.AcquireTokenInteractive(_scopes)
                        .ExecuteAsync();
                    Console.WriteLine("Token acquired interactively.");
                }
            }
            else
            {
                // No cached account, acquire token interactively
                result = await _app.AcquireTokenInteractive(_scopes)
                    .ExecuteAsync();
                Console.WriteLine("Token acquired interactively.");
            }

            Console.WriteLine($"Token expires: {result.ExpiresOn}");
            return result.AccessToken;
        }
        catch (MsalException ex)
        {
            Console.WriteLine($"MSAL Error: {ex.Message}");
            return null;
        }
    }

    private static async Task SendMessageAsync(string accessToken)
    {
        try
        {
            Console.Write("Enter your message: ");
            var message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("Message cannot be empty.");
                return;
            }

            var requestData = new { Message = message };
            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/Message/echo", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<MessageResponse>(responseContent, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Console.WriteLine("\nServer Response:");
                Console.WriteLine($"Original Message: {responseData?.OriginalMessage}");
                Console.WriteLine($"Echo Message: {responseData?.EchoMessage}");
                Console.WriteLine($"Received At: {responseData?.ReceivedAt}");
                Console.WriteLine($"User Name: {responseData?.UserName}");
            }
            else
            {
                Console.WriteLine($"API call failed: {response.StatusCode}");
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }
    }

    private static async Task GetProtectedDataAsync(string accessToken)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/Message/protected");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<JsonElement>(responseContent);

                Console.WriteLine("\nProtected Data:");
                Console.WriteLine($"Message: {responseData.GetProperty("message").GetString()}");
                Console.WriteLine($"User: {responseData.GetProperty("user").GetString()}");
                Console.WriteLine($"Timestamp: {responseData.GetProperty("timestamp").GetDateTime()}");
            }
            else
            {
                Console.WriteLine($"API call failed: {response.StatusCode}");
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting protected data: {ex.Message}");
        }
    }
}

public class MessageResponse
{
    public string OriginalMessage { get; set; } = string.Empty;
    public string EchoMessage { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
    public string UserName { get; set; } = string.Empty;
}