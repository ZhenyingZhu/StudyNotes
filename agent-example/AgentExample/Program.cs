using System.ClientModel;
using Microsoft.Agents.AI;
using OpenAI;

namespace AgentExample;

public static class Program
{
    public static async Task Main(string[] args)
    {
        const string AgentName = "MyAgent";
        const string AgentInstructions = "You are a helpful agent.";
        
        // TODO: Replace with your GitHub Token and Model ID
        // You can get a token from https://github.com/settings/tokens
        // You can find available models at https://github.com/marketplace/models
        string? githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

        if (string.IsNullOrWhiteSpace(githubToken))
        {
            Console.Write("Please enter your GitHub Token: ");
            githubToken = Console.ReadLine();
        }

        if (string.IsNullOrWhiteSpace(githubToken))
        {
            Console.WriteLine("Error: GitHub Token is required.");
            return;
        }

        string modelId = "gpt-4o"; 

        // Create OpenAIClient based ChatAgent
        AIAgent agent = new OpenAIClient(
            new ApiKeyCredential(githubToken),
            new OpenAIClientOptions
            {
                Endpoint = new Uri("https://models.github.ai/inference")
            }
        )
        .GetChatClient(modelId)
        .CreateAIAgent(
            AgentInstructions,
            AgentName
        );

        // Run the prompt 3 times
        for (int i = 1; i <= 3; i++)
        {
            Console.WriteLine($"\n--- Run {i} ---");
            Console.Write("Agent: ");
            
            // Using a simple prompt "Tell me a joke" for demonstration
            await foreach (var update in agent.RunStreamingAsync("Tell me a joke"))
            {
                if (!string.IsNullOrEmpty(update.Text))
                {
                    Console.Write(update.Text);
                }
            }
            Console.WriteLine();
        }
    }
}
