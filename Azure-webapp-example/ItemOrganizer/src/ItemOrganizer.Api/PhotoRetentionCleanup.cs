using Azure;
using ItemOrganizer.Domain;
using ItemOrganizer.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ItemOrganizer.Api;

public sealed class PhotoRetentionCleanup(
    ItemOrganizerDbContext dbContext,
    IPhotoStorage storage,
    ILogger<PhotoRetentionCleanup> logger)
{
    public async Task RunOnceAsync(
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var deletedItemsWithCrops = await dbContext.Items
            .Where(item =>
                item.DeletedAt != null
                && item.CropDeletionPendingAt != null
                && item.CropBlobName != null)
            .OrderBy(item => item.CropDeletionPendingAt)
            .Take(100)
            .ToListAsync(cancellationToken);
        foreach (var item in deletedItemsWithCrops)
        {
            try
            {
                await storage.DeleteIfExistsAsync(
                    item.CropBlobName!,
                    cancellationToken);
                item.MarkCropDeleted(now);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (RequestFailedException exception)
            {
                logger.LogError(
                    exception,
                    "Retention cleanup could not delete crop for item {ItemId}.",
                    item.Id);
            }
        }

        var photos = await dbContext.Photos
            .Where(photo =>
                photo.DeletedAt == null
                && (photo.RetentionState == PhotoRetentionState.PendingDeletion
                    || photo.RetainUntil <= now))
            .OrderBy(photo => photo.RetainUntil)
            .Take(100)
            .ToListAsync(cancellationToken);

        foreach (var photo in photos)
        {
            var isInUse = await dbContext.Analyses.AnyAsync(
                    analysis =>
                        analysis.PhotoId == photo.Id
                        && (analysis.Status == AnalysisStatus.Queued
                            || analysis.Status == AnalysisStatus.Running),
                    cancellationToken)
                || await dbContext.Items.AnyAsync(
                    item =>
                        item.PhotoId == photo.Id
                        && item.DeletedAt == null,
                    cancellationToken);
            if (isInUse)
            {
                continue;
            }

            if (photo.RetentionState == PhotoRetentionState.Active)
            {
                photo.RequestDeletion(false, now);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            try
            {
                await storage.DeleteIfExistsAsync(
                    photo.BlobName,
                    cancellationToken);
                photo.MarkDeleted(now);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (RequestFailedException exception)
            {
                logger.LogError(
                    exception,
                    "Retention cleanup could not delete photo {PhotoId}.",
                    photo.Id);
            }
        }
    }
}

public sealed class PhotoRetentionWorker(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<PhotoRetentionWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!configuration.GetValue("PhotoStorage:CleanupEnabled", true))
        {
            return;
        }

        var interval = TimeSpan.FromMinutes(
            Math.Clamp(
                configuration.GetValue("PhotoStorage:CleanupIntervalMinutes", 5),
                1,
                15));
        using var timer = new PeriodicTimer(interval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var cleanup =
                    scope.ServiceProvider.GetRequiredService<PhotoRetentionCleanup>();
                await cleanup.RunOnceAsync(
                    DateTimeOffset.UtcNow,
                    stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Photo retention cleanup failed.");
            }
        }
    }
}
