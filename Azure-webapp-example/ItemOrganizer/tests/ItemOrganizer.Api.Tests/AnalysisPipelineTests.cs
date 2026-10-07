using ItemOrganizer.Api;
using ItemOrganizer.Domain;
using ItemOrganizer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace ItemOrganizer.Api.Tests;

public sealed class AnalysisPipelineTests
{
    [Fact]
    public async Task Deterministic_result_creates_review_drafts_and_is_idempotent()
    {
        await using var dbContext = CreateContext();
        var setup = await SeedQueuedAnalysisAsync(dbContext);
        var processor = CreateProcessor(
            dbContext,
            new DeterministicAnalysisProvider());

        var first = await processor.ProcessAsync(
            setup.AnalysisId,
            "correlation-1",
            CancellationToken.None);
        var second = await processor.ProcessAsync(
            setup.AnalysisId,
            "correlation-2",
            CancellationToken.None);

        Assert.Equal(
            AnalysisProcessingDisposition.Complete,
            first.Disposition);
        Assert.Equal(
            AnalysisProcessingDisposition.Complete,
            second.Disposition);
        var analysis = await dbContext.Analyses.SingleAsync(
            entity => entity.Id == setup.AnalysisId);
        var detections = await dbContext.AnalysisDetections
            .Where(detection => detection.AnalysisId == setup.AnalysisId)
            .ToListAsync();
        Assert.Equal(AnalysisStatus.Completed, analysis.Status);
        Assert.Equal(2, detections.Count);
        Assert.All(
            detections,
            detection => Assert.Equal(
                DetectionReviewStatus.Pending,
                detection.ReviewStatus));
        Assert.False(await dbContext.Items.AnyAsync(
            item => item.AnalysisId == setup.AnalysisId));
    }

    [Fact]
    public async Task Convenience_result_keeps_detections_out_of_inventory()
    {
        await using var dbContext = CreateContext();
        var setup = await SeedQueuedAnalysisAsync(
            dbContext,
            useConfirmedContainer: true);
        var processor = CreateProcessor(
            dbContext,
            new DeterministicAnalysisProvider());

        var result = await processor.ProcessAsync(
            setup.AnalysisId,
            "correlation-confirmed",
            CancellationToken.None);

        Assert.Equal(
            AnalysisProcessingDisposition.Complete,
            result.Disposition);
        var detections = await dbContext.AnalysisDetections
            .Where(detection => detection.AnalysisId == setup.AnalysisId)
            .ToListAsync();
        Assert.NotEmpty(detections);
        Assert.False(await dbContext.Items.AnyAsync(
            item => item.AnalysisId == setup.AnalysisId));
        var analysis = await dbContext.Analyses.SingleAsync(
            entity => entity.Id == setup.AnalysisId);
        Assert.Equal(setup.ContainerId, analysis.ConfirmedContainerId);
    }

    [Fact]
    public async Task Invalid_provider_output_fails_without_creating_items()
    {
        await using var dbContext = CreateContext();
        var setup = await SeedQueuedAnalysisAsync(dbContext);
        var provider = new StubProvider(request => new(
            "wrong-schema",
            request.PhotoId,
            request.PromptVersion,
            request.Model,
            [],
            []));
        var processor = CreateProcessor(dbContext, provider);

        var result = await processor.ProcessAsync(
            setup.AnalysisId,
            "correlation-invalid",
            CancellationToken.None);

        Assert.Equal(
            AnalysisProcessingDisposition.DeadLetter,
            result.Disposition);
        var analysis = await dbContext.Analyses.SingleAsync(
            entity => entity.Id == setup.AnalysisId);
        Assert.Equal(AnalysisStatus.Failed, analysis.Status);
        Assert.Equal("AI_OUTPUT_INVALID", analysis.ErrorCode);
        Assert.False(await dbContext.Items.AnyAsync(
            item => item.AnalysisId == setup.AnalysisId));
    }

    [Fact]
    public async Task Cancellation_after_provider_call_discards_results()
    {
        await using var dbContext = CreateContext();
        var setup = await SeedQueuedAnalysisAsync(dbContext);
        var processor = CreateProcessor(
            dbContext,
            new CancellingProvider(dbContext));

        var result = await processor.ProcessAsync(
            setup.AnalysisId,
            "correlation-cancel",
            CancellationToken.None);

        Assert.Equal(
            AnalysisProcessingDisposition.Complete,
            result.Disposition);
        var analysis = await dbContext.Analyses.SingleAsync(
            entity => entity.Id == setup.AnalysisId);
        Assert.Equal(AnalysisStatus.Cancelled, analysis.Status);
        Assert.False(await dbContext.Items.AnyAsync(
            item => item.AnalysisId == setup.AnalysisId));
    }

    [Fact]
    public async Task Transient_failures_retry_five_times_then_dead_letter()
    {
        await using var dbContext = CreateContext();
        var setup = await SeedQueuedAnalysisAsync(dbContext);
        var processor = CreateProcessor(
            dbContext,
            new ThrowingProvider());

        for (var attempt = 1; attempt <= Analysis.MaximumDeliveryAttempts; attempt++)
        {
            var result = await processor.ProcessAsync(
                setup.AnalysisId,
                $"correlation-{attempt}",
                CancellationToken.None);
            var expected = attempt < Analysis.MaximumDeliveryAttempts
                ? AnalysisProcessingDisposition.Retry
                : AnalysisProcessingDisposition.DeadLetter;
            Assert.Equal(expected, result.Disposition);
        }

        var analysis = await dbContext.Analyses.SingleAsync(
            entity => entity.Id == setup.AnalysisId);
        Assert.Equal(AnalysisStatus.Failed, analysis.Status);
        Assert.Equal(
            Analysis.MaximumDeliveryAttempts,
            analysis.DeliveryAttemptCount);
        Assert.Equal(
            "AI_PROVIDER_TEMPORARY_FAILURE",
            analysis.ErrorCode);
    }

    [Fact]
    public async Task Outbox_dispatch_enqueues_once_and_marks_message()
    {
        await using var dbContext = CreateContext();
        var setup = await SeedQueuedAnalysisAsync(dbContext);
        var queue = new RecordingQueue();
        var dispatcher = new AnalysisOutboxDispatcher(
            dbContext,
            queue,
            NullLogger<AnalysisOutboxDispatcher>.Instance);

        await dispatcher.RunOnceAsync(
            DateTimeOffset.UtcNow.AddMinutes(1),
            CancellationToken.None);
        await dispatcher.RunOnceAsync(
            DateTimeOffset.UtcNow.AddMinutes(2),
            CancellationToken.None);

        Assert.Equal([setup.AnalysisId], queue.Enqueued);
        var message = await dbContext.OutboxMessages.SingleAsync(
            entity => entity.AnalysisId == setup.AnalysisId);
        Assert.Equal(OutboxStatus.Dispatched, message.Status);
    }

    [Fact]
    public async Task Outbox_dispatch_failure_is_bounded_and_fails_analysis()
    {
        await using var dbContext = CreateContext();
        var setup = await SeedQueuedAnalysisAsync(dbContext);
        var dispatcher = new AnalysisOutboxDispatcher(
            dbContext,
            new FailingQueue(),
            NullLogger<AnalysisOutboxDispatcher>.Instance);
        var now = DateTimeOffset.UtcNow;

        for (var attempt = 1; attempt <= Analysis.MaximumDeliveryAttempts; attempt++)
        {
            await dispatcher.RunOnceAsync(
                now.AddHours(attempt),
                CancellationToken.None);
        }

        var message = await dbContext.OutboxMessages.SingleAsync(
            entity => entity.AnalysisId == setup.AnalysisId);
        var analysis = await dbContext.Analyses.SingleAsync(
            entity => entity.Id == setup.AnalysisId);
        Assert.Equal(OutboxStatus.Failed, message.Status);
        Assert.Equal(
            Analysis.MaximumDeliveryAttempts,
            message.AttemptCount);
        Assert.Equal(AnalysisStatus.Failed, analysis.Status);
        Assert.Equal("QUEUE_DISPATCH_FAILED", analysis.ErrorCode);
    }

    private static ItemOrganizerDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ItemOrganizerDbContext>()
            .UseInMemoryDatabase($"analysis-{Guid.NewGuid():N}")
            .Options;
        return new ItemOrganizerDbContext(options);
    }

    private static async Task<(Guid AnalysisId, Guid ContainerId)>
        SeedQueuedAnalysisAsync(
            ItemOrganizerDbContext dbContext,
            bool useConfirmedContainer = false)
    {
        var now = DateTimeOffset.UtcNow;
        var photo = new Photo(
            Guid.NewGuid(),
            ApiFactory.TenantId,
            ApiFactory.OwnerId,
            $"photos/{Guid.NewGuid():N}",
            "image/png",
            1024,
            512,
            512,
            new string('a', 64),
            now.AddDays(30),
            now);
        var container = new StorageContainer(
            Guid.NewGuid(),
            ApiFactory.TenantId,
            ApiFactory.OwnerId,
            "Cables",
            "Electronic cables",
            "Office",
            ["electronics"],
            now);
        var analysis = new Analysis(
            Guid.NewGuid(),
            ApiFactory.TenantId,
            ApiFactory.OwnerId,
            photo.Id,
            "prompt-v1",
            "schema-v1",
            "mock-v1",
            "tests",
            useConfirmedContainer ? container.Id : null,
            now);
        var outbox = new OutboxMessage(
            Guid.NewGuid(),
            ApiFactory.TenantId,
            ApiFactory.OwnerId,
            analysis.Id,
            now);
        dbContext.AddRange(photo, container, analysis, outbox);
        await dbContext.SaveChangesAsync();
        return (analysis.Id, container.Id);
    }

    private static AnalysisProcessor CreateProcessor(
        ItemOrganizerDbContext dbContext,
        IAnalysisProvider provider)
    {
        return new(
            dbContext,
            provider,
            new AnalysisResultPersistence(dbContext),
            NullLogger<AnalysisProcessor>.Instance);
    }

    private sealed class StubProvider(
        Func<AnalysisProviderRequest, AnalysisProviderResult> factory)
        : IAnalysisProvider
    {
        public Task<AnalysisProviderResult> AnalyzeAsync(
            AnalysisProviderRequest request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(factory(request));
        }
    }

    private sealed class ThrowingProvider : IAnalysisProvider
    {
        public Task<AnalysisProviderResult> AnalyzeAsync(
            AnalysisProviderRequest request,
            CancellationToken cancellationToken)
        {
            throw new AnalysisProviderTransientException(
                "Simulated temporary failure.");
        }
    }

    private sealed class CancellingProvider(ItemOrganizerDbContext dbContext)
        : IAnalysisProvider
    {
        public async Task<AnalysisProviderResult> AnalyzeAsync(
            AnalysisProviderRequest request,
            CancellationToken cancellationToken)
        {
            var analysis = await dbContext.Analyses.SingleAsync(
                entity => entity.Id == request.AnalysisId,
                cancellationToken);
            analysis.RequestCancellation(DateTimeOffset.UtcNow);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new(
                request.SchemaVersion,
                request.PhotoId,
                request.PromptVersion,
                request.Model,
                [
                    new(
                        "Discarded",
                        null,
                        null,
                        1,
                        0.9m,
                        null,
                        null)
                ],
                []);
        }
    }

    private class RecordingQueue : IAnalysisQueue
    {
        public List<Guid> Enqueued { get; } = [];

        public virtual Task EnqueueAsync(
            Guid analysisId,
            CancellationToken cancellationToken)
        {
            Enqueued.Add(analysisId);
            return Task.CompletedTask;
        }

        public Task<AnalysisQueueMessage?> ReceiveAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult<AnalysisQueueMessage?>(null);
        }

        public Task CompleteAsync(
            AnalysisQueueMessage message,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task RetryAsync(
            AnalysisQueueMessage message,
            TimeSpan delay,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task DeadLetterAsync(
            AnalysisQueueMessage message,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FailingQueue : RecordingQueue
    {
        public override Task EnqueueAsync(
            Guid analysisId,
            CancellationToken cancellationToken)
        {
            throw new IOException("Simulated queue failure.");
        }
    }
}
