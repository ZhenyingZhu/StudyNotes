using ItemOrganizer.Api;
using ItemOrganizer.Domain;
using ItemOrganizer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace ItemOrganizer.Api.Tests;

public sealed class PhotoRetentionCleanupTests
{
    [Fact]
    public async Task Expired_unreferenced_photo_is_hard_deleted()
    {
        var options = new DbContextOptionsBuilder<ItemOrganizerDbContext>()
            .UseInMemoryDatabase($"retention-{Guid.NewGuid():N}")
            .Options;
        await using var dbContext = new ItemOrganizerDbContext(options);
        var storage = new FakePhotoStorage();
        var now = new DateTimeOffset(
            2026,
            9,
            25,
            12,
            0,
            0,
            TimeSpan.Zero);
        var photo = new Photo(
            Guid.NewGuid(),
            ApiFactory.TenantId,
            ApiFactory.OwnerId,
            "photos/expired",
            "image/png",
            100,
            512,
            512,
            new string('a', 64),
            now.AddMinutes(-1),
            now.AddDays(-30));
        dbContext.Photos.Add(photo);
        await dbContext.SaveChangesAsync();
        storage.Seed(photo.BlobName, [1, 2, 3], "image/png");

        var cleanup = new PhotoRetentionCleanup(
            dbContext,
            storage,
            NullLogger<PhotoRetentionCleanup>.Instance);
        await cleanup.RunOnceAsync(now, CancellationToken.None);

        Assert.Equal(PhotoRetentionState.Deleted, photo.RetentionState);
        Assert.NotNull(photo.DeletedAt);
        Assert.Empty(storage.Photos);
    }

    [Fact]
    public async Task Storage_failure_leaves_photo_pending_for_retry()
    {
        var options = new DbContextOptionsBuilder<ItemOrganizerDbContext>()
            .UseInMemoryDatabase($"retention-{Guid.NewGuid():N}")
            .Options;
        await using var dbContext = new ItemOrganizerDbContext(options);
        var storage = new FakePhotoStorage { FailDeletes = true };
        var now = DateTimeOffset.UtcNow;
        var photo = new Photo(
            Guid.NewGuid(),
            ApiFactory.TenantId,
            ApiFactory.OwnerId,
            "photos/retry",
            "image/png",
            100,
            512,
            512,
            new string('b', 64),
            now.AddMinutes(-1),
            now.AddDays(-30));
        dbContext.Photos.Add(photo);
        await dbContext.SaveChangesAsync();

        var cleanup = new PhotoRetentionCleanup(
            dbContext,
            storage,
            NullLogger<PhotoRetentionCleanup>.Instance);
        await cleanup.RunOnceAsync(now, CancellationToken.None);

        Assert.Equal(
            PhotoRetentionState.PendingDeletion,
            photo.RetentionState);
        Assert.Null(photo.DeletedAt);
    }

    [Fact]
    public async Task Deleted_item_crop_is_removed_and_metadata_is_cleared()
    {
        var options = new DbContextOptionsBuilder<ItemOrganizerDbContext>()
            .UseInMemoryDatabase($"retention-{Guid.NewGuid():N}")
            .Options;
        await using var dbContext = new ItemOrganizerDbContext(options);
        var storage = new FakePhotoStorage();
        var now = DateTimeOffset.UtcNow;
        var item = Item.CreateManual(
            Guid.NewGuid(),
            ApiFactory.TenantId,
            ApiFactory.OwnerId,
            "Tape",
            null,
            "Supplies",
            1,
            now.AddDays(-1));
        item.SetAssignment(ItemAssignment.Unassigned(
            Guid.NewGuid(),
            ApiFactory.TenantId,
            ApiFactory.OwnerId,
            item.Id,
            now.AddDays(-1)));
        item.AttachCrop(
            "crops/tape.png",
            new NormalizedBoundingBox(0.1m, 0.1m, 0.5m, 0.5m),
            256,
            256,
            "image/png",
            3,
            new string('c', 64));
        item.MarkDeleted(now);
        dbContext.Items.Add(item);
        await dbContext.SaveChangesAsync();
        storage.Seed(item.CropBlobName!, [1, 2, 3], "image/png");

        var cleanup = new PhotoRetentionCleanup(
            dbContext,
            storage,
            NullLogger<PhotoRetentionCleanup>.Instance);
        await cleanup.RunOnceAsync(now.AddMinutes(1), CancellationToken.None);

        Assert.Null(item.CropBlobName);
        Assert.Null(item.CropDeletionPendingAt);
        Assert.Empty(storage.Photos);
    }
}
