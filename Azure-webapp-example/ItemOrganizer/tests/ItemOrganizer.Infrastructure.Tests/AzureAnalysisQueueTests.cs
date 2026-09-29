using Azure.Storage.Queues;
using ItemOrganizer.Api;
using Microsoft.Extensions.Configuration;

namespace ItemOrganizer.Infrastructure.Tests;

public sealed class AzureAnalysisQueueTests
{
    [Fact]
    public async Task Queue_message_can_be_completed_or_dead_lettered()
    {
        var connectionString = Environment.GetEnvironmentVariable(
            "ITEMORGANIZER_STORAGE_CONNECTION");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "ITEMORGANIZER_STORAGE_CONNECTION is required for queue integration tests.");
        }

        var suffix = Guid.NewGuid().ToString("N");
        var queueName = $"analyses-{suffix}";
        var deadLetterQueueName = $"analyses-dead-{suffix}";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Analysis:QueueName"] = queueName,
                ["Analysis:DeadLetterQueueName"] = deadLetterQueueName
            })
            .Build();
        var queue = new AzureStorageAnalysisQueue(
            connectionString,
            configuration);
        var queueOptions = new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        };
        var queueClient = new QueueClient(
            connectionString,
            queueName,
            queueOptions);
        var deadLetterQueueClient = new QueueClient(
            connectionString,
            deadLetterQueueName,
            queueOptions);

        try
        {
            var completedId = Guid.NewGuid();
            await queue.EnqueueAsync(completedId, CancellationToken.None);
            var completed = await queue.ReceiveAsync(CancellationToken.None);
            Assert.NotNull(completed);
            Assert.Equal(completedId, completed.AnalysisId);
            await queue.CompleteAsync(completed, CancellationToken.None);

            var deadLetteredId = Guid.NewGuid();
            await queue.EnqueueAsync(deadLetteredId, CancellationToken.None);
            var deadLettered = await queue.ReceiveAsync(CancellationToken.None);
            Assert.NotNull(deadLettered);
            await queue.DeadLetterAsync(
                deadLettered,
                CancellationToken.None);

            var deadLetterMessages =
                await deadLetterQueueClient.ReceiveMessagesAsync(1);
            Assert.Single(deadLetterMessages.Value);
            Assert.Equal(
                deadLetteredId.ToString("D"),
                deadLetterMessages.Value[0].MessageText);
            Assert.Empty((await queueClient.PeekMessagesAsync(1)).Value);
        }
        finally
        {
            await queueClient.DeleteIfExistsAsync();
            await deadLetterQueueClient.DeleteIfExistsAsync();
        }
    }
}
