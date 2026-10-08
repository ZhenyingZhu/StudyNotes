using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using ItemOrganizer.Domain;
using ItemOrganizer.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ItemOrganizer.Api;

public sealed record AnalysisProviderRequest(
    Guid AnalysisId,
    Guid PhotoId,
    string PhotoBlobName,
    string PhotoContentType,
    string PromptVersion,
    string SchemaVersion,
    string Model,
    IReadOnlyList<AnalysisContainer> Containers);

public sealed record AnalysisContainer(
    Guid Id,
    string Name,
    string? Description,
    string? Location,
    IReadOnlyList<string> Labels);

public sealed record AnalysisProviderResult(
    string SchemaVersion,
    Guid PhotoId,
    string PromptVersion,
    string Model,
    IReadOnlyList<AnalysisProviderItem> Items,
    IReadOnlyList<string> Warnings);

public sealed record AnalysisProviderItem(
    string Name,
    string? Description,
    string? Category,
    int Quantity,
    decimal Confidence,
    Guid? SuggestedContainerId,
    string? SuggestedContainerReason,
    NormalizedBoundingBox? BoundingBox = null);

public interface IAnalysisProvider
{
    Task<AnalysisProviderResult> AnalyzeAsync(
        AnalysisProviderRequest request,
        CancellationToken cancellationToken);
}

public sealed class DeterministicAnalysisProvider : IAnalysisProvider
{
    public Task<AnalysisProviderResult> AnalyzeAsync(
        AnalysisProviderRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var suggestedContainerId = request.Containers
            .OrderBy(container => container.Name, StringComparer.Ordinal)
            .ThenBy(container => container.Id)
            .Select(container => (Guid?)container.Id)
            .FirstOrDefault();

        return Task.FromResult(new AnalysisProviderResult(
            request.SchemaVersion,
            request.PhotoId,
            request.PromptVersion,
            request.Model,
            [
                new(
                    "USB-C cable",
                    "Black braided cable",
                    "Electronics",
                    2,
                    0.92m,
                    suggestedContainerId,
                    suggestedContainerId is null
                        ? null
                        : "The container metadata is compatible with electronics.",
                    new(0.08m, 0.15m, 0.40m, 0.22m)),
                new(
                    "Sticky notes",
                    "Yellow note pad",
                    "Stationery",
                    1,
                    0.73m,
                    null,
                    null,
                    new(0.56m, 0.42m, 0.30m, 0.28m))
            ],
            []));
    }
}

public sealed class AnalysisProviderTransientException(
    string message,
    Exception? innerException = null) : Exception(message, innerException);

public sealed class AnalysisProviderRejectedException(
    string message,
    Exception? innerException = null) : Exception(message, innerException);

public sealed class AnalysisProviderInvalidOutputException(
    string message,
    Exception? innerException = null) : Exception(message, innerException);

public sealed record AnalysisQueueMessage(
    Guid AnalysisId,
    string MessageId,
    string PopReceipt,
    long DequeueCount);

public interface IAnalysisQueue
{
    Task EnqueueAsync(Guid analysisId, CancellationToken cancellationToken);

    Task<AnalysisQueueMessage?> ReceiveAsync(
        CancellationToken cancellationToken);

    Task CompleteAsync(
        AnalysisQueueMessage message,
        CancellationToken cancellationToken);

    Task RetryAsync(
        AnalysisQueueMessage message,
        TimeSpan delay,
        CancellationToken cancellationToken);

    Task DeadLetterAsync(
        AnalysisQueueMessage message,
        CancellationToken cancellationToken);
}

public sealed class AzureStorageAnalysisQueue : IAnalysisQueue
{
    private readonly QueueClient _queue;
    private readonly QueueClient _deadLetterQueue;

    public AzureStorageAnalysisQueue(
        string connectionString,
        IConfiguration configuration)
    {
        var options = new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        };
        _queue = new QueueClient(
            connectionString,
            configuration["Analysis:QueueName"]
                ?? "item-organizer-analyses",
            options);
        _deadLetterQueue = new QueueClient(
            connectionString,
            configuration["Analysis:DeadLetterQueueName"]
                ?? "item-organizer-analyses-deadletter",
            options);
    }

    public async Task EnqueueAsync(
        Guid analysisId,
        CancellationToken cancellationToken)
    {
        await _queue.CreateIfNotExistsAsync(
            cancellationToken: cancellationToken);
        await _queue.SendMessageAsync(
            analysisId.ToString("D"),
            cancellationToken);
    }

    public async Task<AnalysisQueueMessage?> ReceiveAsync(
        CancellationToken cancellationToken)
    {
        await _queue.CreateIfNotExistsAsync(
            cancellationToken: cancellationToken);
        var response = await _queue.ReceiveMessagesAsync(
            maxMessages: 1,
            visibilityTimeout: TimeSpan.FromMinutes(2),
            cancellationToken: cancellationToken);
        var message = response.Value.FirstOrDefault();
        if (message is null)
        {
            return null;
        }

        if (!Guid.TryParse(message.MessageText, out var analysisId))
        {
            await MoveInvalidMessageToDeadLetterAsync(
                message,
                cancellationToken);
            return null;
        }

        return new(
            analysisId,
            message.MessageId,
            message.PopReceipt,
            message.DequeueCount);
    }

    public async Task CompleteAsync(
        AnalysisQueueMessage message,
        CancellationToken cancellationToken)
    {
        await _queue.DeleteMessageAsync(
            message.MessageId,
            message.PopReceipt,
            cancellationToken);
    }

    public async Task RetryAsync(
        AnalysisQueueMessage message,
        TimeSpan delay,
        CancellationToken cancellationToken)
    {
        await _queue.UpdateMessageAsync(
            message.MessageId,
            message.PopReceipt,
            message.AnalysisId.ToString("D"),
            delay,
            cancellationToken);
    }

    public async Task DeadLetterAsync(
        AnalysisQueueMessage message,
        CancellationToken cancellationToken)
    {
        await _deadLetterQueue.CreateIfNotExistsAsync(
            cancellationToken: cancellationToken);
        await _deadLetterQueue.SendMessageAsync(
            message.AnalysisId.ToString("D"),
            cancellationToken);
        await CompleteAsync(message, cancellationToken);
    }

    private async Task MoveInvalidMessageToDeadLetterAsync(
        QueueMessage message,
        CancellationToken cancellationToken)
    {
        await _deadLetterQueue.CreateIfNotExistsAsync(
            cancellationToken: cancellationToken);
        await _deadLetterQueue.SendMessageAsync(
            message.MessageText,
            cancellationToken);
        await _queue.DeleteMessageAsync(
            message.MessageId,
            message.PopReceipt,
            cancellationToken);
    }
}

public sealed class AnalysisOutboxDispatcher(
    ItemOrganizerDbContext dbContext,
    IAnalysisQueue queue,
    ILogger<AnalysisOutboxDispatcher> logger)
{
    private const int MaximumAttempts = 6;

    public async Task<int> RunOnceAsync(
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var messages = await dbContext.OutboxMessages
            .Where(message =>
                message.Status == OutboxStatus.Pending
                && message.AvailableAt <= now)
            .OrderBy(message => message.CreatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                await queue.EnqueueAsync(
                    message.AnalysisId,
                    cancellationToken);
                message.MarkDispatched(now);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception) when (
                exception is RequestFailedException
                    or TimeoutException
                    or IOException)
            {
                await RecordFailureAsync(
                    message,
                    exception,
                    now,
                    cancellationToken);
            }
            catch (OperationCanceledException exception) when (
                !cancellationToken.IsCancellationRequested)
            {
                await RecordFailureAsync(
                    message,
                    exception,
                    now,
                    cancellationToken);
            }
        }

        return messages.Count;
    }

    private async Task RecordFailureAsync(
        OutboxMessage message,
        Exception exception,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        logger.LogWarning(
            exception,
            "Analysis outbox dispatch failed for {AnalysisId}.",
            message.AnalysisId);
        if (message.AttemptCount + 1 >= MaximumAttempts)
        {
            message.MarkFailed("QUEUE_DISPATCH_FAILED", now);
            var analysis = await dbContext.Analyses.SingleAsync(
                entity => entity.Id == message.AnalysisId,
                cancellationToken);
            if (analysis.Status == AnalysisStatus.Queued)
            {
                analysis.Fail(
                    "QUEUE_DISPATCH_FAILED",
                    "The analysis could not be queued. Retry the analysis later.",
                    message.Id.ToString("D"),
                    now);
            }
        }
        else
        {
            message.ScheduleRetry(
                "QUEUE_DISPATCH_TEMPORARY_FAILURE",
                now.Add(Backoff(message.AttemptCount + 1)));
        }
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static TimeSpan Backoff(int attempt)
    {
        return TimeSpan.FromSeconds(
            Math.Min(300, Math.Pow(2, Math.Max(0, attempt - 1))));
    }
}

public enum AnalysisProcessingDisposition
{
    Complete,
    Retry,
    DeadLetter
}

public sealed record AnalysisProcessingResult(
    AnalysisProcessingDisposition Disposition,
    TimeSpan RetryDelay)
{
    public static AnalysisProcessingResult Complete() =>
        new(AnalysisProcessingDisposition.Complete, TimeSpan.Zero);

    public static AnalysisProcessingResult Retry(TimeSpan delay) =>
        new(AnalysisProcessingDisposition.Retry, delay);

    public static AnalysisProcessingResult DeadLetter() =>
        new(AnalysisProcessingDisposition.DeadLetter, TimeSpan.Zero);
}

public sealed class AnalysisProcessor(
    ItemOrganizerDbContext dbContext,
    IAnalysisProvider provider,
    AnalysisResultPersistence persistence,
    ILogger<AnalysisProcessor> logger)
{
    public async Task<AnalysisProcessingResult> ProcessAsync(
        Guid analysisId,
        string correlationId,
        CancellationToken cancellationToken)
    {
        var analysis = await dbContext.Analyses.SingleOrDefaultAsync(
            entity => entity.Id == analysisId,
            cancellationToken);
        if (analysis is null
            || analysis.Status is AnalysisStatus.Completed
                or AnalysisStatus.Failed
                or AnalysisStatus.Cancelled)
        {
            return AnalysisProcessingResult.Complete();
        }

        try
        {
            if (analysis.Status == AnalysisStatus.Queued)
            {
                analysis.Start(DateTimeOffset.UtcNow);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            if (analysis.CancellationRequestedAt is not null)
            {
                analysis.CancelFromWorker(DateTimeOffset.UtcNow);
                await dbContext.SaveChangesAsync(cancellationToken);
                return AnalysisProcessingResult.Complete();
            }

            var containers = await dbContext.Containers
                .AsNoTracking()
                .Where(container =>
                    container.TenantId == analysis.TenantId
                    && container.OwnerObjectId == analysis.OwnerObjectId
                    && container.DeletedAt == null)
                .OrderBy(container => container.Name)
                .Select(container => new AnalysisContainer(
                    container.Id,
                    container.Name,
                    container.Description,
                    container.Location,
                    container.Labels))
                .ToListAsync(cancellationToken);
            var photo = await dbContext.Photos
                .AsNoTracking()
                .Where(entity =>
                    entity.Id == analysis.PhotoId
                    && entity.TenantId == analysis.TenantId
                    && entity.OwnerObjectId == analysis.OwnerObjectId
                    && entity.DeletedAt == null)
                .Select(entity => new
                {
                    entity.BlobName,
                    entity.ContentType
                })
                .SingleOrDefaultAsync(cancellationToken);
            if (photo is null)
            {
                throw new AnalysisProviderRejectedException(
                    "The analysis photo is unavailable.");
            }
            var result = await provider.AnalyzeAsync(
                new(
                    analysis.Id,
                    analysis.PhotoId,
                    photo.BlobName,
                    photo.ContentType,
                    analysis.PromptVersion,
                    analysis.SchemaVersion,
                    analysis.Model,
                    containers),
                cancellationToken);

            await dbContext.Entry(analysis).ReloadAsync(cancellationToken);
            if (analysis.CancellationRequestedAt is not null)
            {
                analysis.CancelFromWorker(DateTimeOffset.UtcNow);
                await dbContext.SaveChangesAsync(cancellationToken);
                return AnalysisProcessingResult.Complete();
            }

            var detections = ValidateResult(analysis, result);
            await persistence.PersistCompletedAnalysisAsync(
                analysis.Id,
                detections,
                result.Warnings,
                DateTimeOffset.UtcNow,
                cancellationToken);
            return AnalysisProcessingResult.Complete();
        }
        catch (AnalysisProviderTransientException exception)
        {
            logger.LogWarning(
                exception,
                "Temporary AI provider failure for analysis {AnalysisId}.",
                analysisId);
            return await RecordTransientFailureAsync(
                analysisId,
                correlationId,
                cancellationToken);
        }
        catch (Exception exception) when (
            exception is TimeoutException or RequestFailedException)
        {
            logger.LogWarning(
                exception,
                "Temporary analysis dependency failure for {AnalysisId}.",
                analysisId);
            return await RecordTransientFailureAsync(
                analysisId,
                correlationId,
                cancellationToken);
        }
        catch (OperationCanceledException exception) when (
            !cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning(
                exception,
                "Analysis provider timed out for {AnalysisId}.",
                analysisId);
            return await RecordTransientFailureAsync(
                analysisId,
                correlationId,
                cancellationToken);
        }
        catch (AnalysisProviderRejectedException exception)
        {
            logger.LogWarning(
                exception,
                "AI provider rejected analysis {AnalysisId}.",
                analysisId);
            await FailAsync(
                analysisId,
                "AI_PROVIDER_REJECTED_REQUEST",
                "The analysis request could not be processed.",
                correlationId,
                cancellationToken);
            return AnalysisProcessingResult.DeadLetter();
        }
        catch (AnalysisProviderInvalidOutputException exception)
        {
            logger.LogWarning(
                exception,
                "Invalid AI output for analysis {AnalysisId}.",
                analysisId);
            await FailAsync(
                analysisId,
                "AI_OUTPUT_INVALID",
                "The analysis provider returned an invalid result.",
                correlationId,
                cancellationToken);
            return AnalysisProcessingResult.DeadLetter();
        }
        catch (InvalidAnalysisOutputException exception)
        {
            logger.LogWarning(
                exception,
                "Invalid AI output for analysis {AnalysisId}.",
                analysisId);
            await FailAsync(
                analysisId,
                "AI_OUTPUT_INVALID",
                "The analysis returned an invalid result.",
                correlationId,
                cancellationToken);
            return AnalysisProcessingResult.DeadLetter();
        }
        catch (DomainException exception)
        {
            logger.LogWarning(
                exception,
                "AI result persistence failed validation for analysis {AnalysisId}.",
                analysisId);
            await FailAsync(
                analysisId,
                "AI_OUTPUT_INVALID",
                "The analysis returned an invalid result.",
                correlationId,
                cancellationToken);
            return AnalysisProcessingResult.DeadLetter();
        }
        catch (DbUpdateConcurrencyException exception)
        {
            logger.LogInformation(
                exception,
                "Analysis {AnalysisId} was concurrently updated.",
                analysisId);
            dbContext.ChangeTracker.Clear();
            var current = await dbContext.Analyses.AsNoTracking()
                .SingleOrDefaultAsync(
                    entity => entity.Id == analysisId,
                    cancellationToken);
            return current is null
                || current.Status is AnalysisStatus.Completed
                    or AnalysisStatus.Failed
                    or AnalysisStatus.Cancelled
                ? AnalysisProcessingResult.Complete()
                : AnalysisProcessingResult.Retry(TimeSpan.FromSeconds(1));
        }
        catch (DbUpdateException exception)
        {
            logger.LogWarning(
                exception,
                "Temporary analysis persistence failure for {AnalysisId}.",
                analysisId);
            return await RecordTransientFailureAsync(
                analysisId,
                correlationId,
                cancellationToken);
        }
    }

    private static IReadOnlyList<DetectedItemDraft> ValidateResult(
        Analysis analysis,
        AnalysisProviderResult result)
    {
        if (!string.Equals(
                result.SchemaVersion,
                analysis.SchemaVersion,
                StringComparison.Ordinal)
            || result.PhotoId != analysis.PhotoId
            || !string.Equals(
                result.PromptVersion,
                analysis.PromptVersion,
                StringComparison.Ordinal)
            || !string.Equals(
                result.Model,
                analysis.Model,
                StringComparison.Ordinal)
            || result.Items is null
            || result.Warnings is null)
        {
            throw new InvalidAnalysisOutputException();
        }

        var detections = new List<DetectedItemDraft>(result.Items.Count);
        foreach (var item in result.Items)
        {
            if (string.IsNullOrWhiteSpace(item.Name)
                || item.Quantity <= 0
                || item.Confidence is < 0 or > 1
                || item.Name.Length > 300
                || item.Description?.Length > 2_000
                || item.Category?.Length > 200)
            {
                throw new InvalidAnalysisOutputException();
            }
            detections.Add(new(
                item.Name,
                item.Description,
                item.Category,
                item.Quantity,
                item.Confidence,
                item.SuggestedContainerId,
                item.BoundingBox));
        }

        if (result.Warnings.Any(warning =>
                string.IsNullOrWhiteSpace(warning)
                || warning.Length > 1_000))
        {
            throw new InvalidAnalysisOutputException();
        }

        return detections;
    }

    private async Task<AnalysisProcessingResult> RecordTransientFailureAsync(
        Guid analysisId,
        string correlationId,
        CancellationToken cancellationToken)
    {
        dbContext.ChangeTracker.Clear();
        var analysis = await dbContext.Analyses.SingleAsync(
            entity => entity.Id == analysisId,
            cancellationToken);
        if (analysis.Status != AnalysisStatus.Running)
        {
            return AnalysisProcessingResult.Complete();
        }

        analysis.RecordTransientFailure(
            "AI_PROVIDER_TEMPORARY_FAILURE",
            "The analysis provider is temporarily unavailable.",
            correlationId,
            DateTimeOffset.UtcNow);
        await dbContext.SaveChangesAsync(cancellationToken);
        return analysis.Status == AnalysisStatus.Failed
            ? AnalysisProcessingResult.DeadLetter()
            : AnalysisProcessingResult.Retry(
                TimeSpan.FromSeconds(
                    Math.Min(
                        300,
                        Math.Pow(2, analysis.DeliveryAttemptCount - 1))));
    }

    private async Task FailAsync(
        Guid analysisId,
        string errorCode,
        string errorMessage,
        string correlationId,
        CancellationToken cancellationToken)
    {
        dbContext.ChangeTracker.Clear();
        var analysis = await dbContext.Analyses.SingleOrDefaultAsync(
            entity => entity.Id == analysisId,
            cancellationToken);
        if (analysis is null
            || analysis.Status is AnalysisStatus.Completed
                or AnalysisStatus.Failed
                or AnalysisStatus.Cancelled)
        {
            return;
        }

        analysis.Fail(
            errorCode,
            errorMessage,
            correlationId,
            DateTimeOffset.UtcNow);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private sealed class InvalidAnalysisOutputException : Exception
    {
    }
}

public sealed class AnalysisPipelineWorker(
    IServiceScopeFactory scopeFactory,
    IAnalysisQueue queue,
    IConfiguration configuration,
    ILogger<AnalysisPipelineWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!configuration.GetValue("Analysis:WorkerEnabled", true))
        {
            return;
        }

        var idleDelay = TimeSpan.FromSeconds(
            Math.Clamp(
                configuration.GetValue("Analysis:PollingSeconds", 2),
                1,
                30));
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var dispatcher = scope.ServiceProvider
                        .GetRequiredService<AnalysisOutboxDispatcher>();
                    await dispatcher.RunOnceAsync(
                        DateTimeOffset.UtcNow,
                        stoppingToken);
                }

                var message = await queue.ReceiveAsync(stoppingToken);
                if (message is null)
                {
                    await Task.Delay(idleDelay, stoppingToken);
                    continue;
                }

                AnalysisProcessingResult result;
                using (var scope = scopeFactory.CreateScope())
                {
                    var processor = scope.ServiceProvider
                        .GetRequiredService<AnalysisProcessor>();
                    result = await processor.ProcessAsync(
                        message.AnalysisId,
                        message.MessageId,
                        stoppingToken);
                }

                if (result.Disposition == AnalysisProcessingDisposition.Retry)
                {
                    await queue.RetryAsync(
                        message,
                        result.RetryDelay,
                        stoppingToken);
                }
                else if (result.Disposition
                         == AnalysisProcessingDisposition.DeadLetter)
                {
                    await queue.DeadLetterAsync(message, stoppingToken);
                }
                else
                {
                    await queue.CompleteAsync(message, stoppingToken);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Analysis pipeline iteration failed.");
                await Task.Delay(idleDelay, stoppingToken);
            }
        }
    }
}

public static class AnalysisPipelineServiceCollectionExtensions
{
    public static IServiceCollection AddAnalysisPipeline(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration[
            "ITEMORGANIZER_STORAGE_CONNECTION"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "ITEMORGANIZER_STORAGE_CONNECTION is required.");
        }

        var providerName = configuration["Analysis:Provider"]
            ?.Trim()
            .ToLowerInvariant()
            ?? "deterministic";
        if (providerName == "azure-openai")
        {
            var model = configuration["Analysis:Model"]?.Trim();
            if (string.IsNullOrWhiteSpace(model)
                || model == "deterministic-mock")
            {
                throw new InvalidOperationException(
                    "Analysis:Model must name an Azure OpenAI deployment when Analysis:Provider is azure-openai.");
            }
            services.AddSingleton<Azure.Core.TokenCredential>(
                new Azure.Identity.DefaultAzureCredential());
            services.AddHttpClient<AzureOpenAiAnalysisProvider>();
            services.AddScoped<IAnalysisProvider>(provider =>
                provider.GetRequiredService<AzureOpenAiAnalysisProvider>());
        }
        else if (providerName == "deterministic")
        {
            services.AddSingleton<
                IAnalysisProvider,
                DeterministicAnalysisProvider>();
        }
        else
        {
            throw new InvalidOperationException(
                "Analysis:Provider must be deterministic or azure-openai.");
        }
        services.AddSingleton<IAnalysisQueue>(provider =>
            new AzureStorageAnalysisQueue(
                connectionString,
                provider.GetRequiredService<IConfiguration>()));
        services.AddScoped<AnalysisResultPersistence>();
        services.AddScoped<AnalysisOutboxDispatcher>();
        services.AddScoped<AnalysisProcessor>();
        services.AddHostedService<AnalysisPipelineWorker>();
        return services;
    }
}
