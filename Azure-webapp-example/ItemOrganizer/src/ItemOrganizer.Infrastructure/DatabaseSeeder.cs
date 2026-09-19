using ItemOrganizer.Domain;
using Microsoft.EntityFrameworkCore;

namespace ItemOrganizer.Infrastructure;

public static class DatabaseSeeder
{
    public static readonly Guid TenantId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid OwnerObjectId = Guid.Parse("20000000-0000-0000-0000-000000000001");

    public static async Task SeedAsync(
        ItemOrganizerDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        if (await dbContext.Containers.AnyAsync(cancellationToken))
        {
            return;
        }

        var createdAt = new DateTimeOffset(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);
        var container = new StorageContainer(
            Guid.Parse("30000000-0000-0000-0000-000000000001"),
            TenantId,
            OwnerObjectId,
            "Office supplies",
            "Deterministic Milestone 2 seed container",
            "Home office",
            ["office", "seed"],
            createdAt);
        var photo = new Photo(
            Guid.Parse("40000000-0000-0000-0000-000000000001"),
            TenantId,
            OwnerObjectId,
            "seed/office-supplies.webp",
            "image/webp",
            1024,
            1024,
            768,
            new string('a', 64),
            createdAt.AddDays(30),
            createdAt);
        var analysis = new Analysis(
            Guid.Parse("50000000-0000-0000-0000-000000000001"),
            TenantId,
            OwnerObjectId,
            photo.Id,
            "2026-09-15.m1",
            "item-organizer.analysis-result.v1",
            "gpt-5.4-mini",
            "milestone-2-seed",
            null,
            createdAt);
        var outboxMessage = new OutboxMessage(
            Guid.Parse("60000000-0000-0000-0000-000000000001"),
            TenantId,
            OwnerObjectId,
            analysis.Id,
            createdAt);
        outboxMessage.MarkDispatched(createdAt.AddSeconds(30));
        var idempotencyRecord = new IdempotencyRecord(
            Guid.Parse("70000000-0000-0000-0000-000000000001"),
            TenantId,
            OwnerObjectId,
            "analysis-create",
            "milestone-2-seed",
            new string('b', 64),
            createdAt.AddHours(24),
            createdAt);
        idempotencyRecord.Complete(
            "analysis",
            analysis.Id,
            createdAt.AddSeconds(30));
        analysis.Start(createdAt.AddMinutes(1));

        dbContext.AddRange(
            container,
            photo,
            analysis,
            outboxMessage,
            idempotencyRecord);
        await dbContext.SaveChangesAsync(cancellationToken);

        var persistence = new AnalysisResultPersistence(dbContext);
        await persistence.PersistCompletedAnalysisAsync(
            analysis.Id,
            [
                new(
                    "USB-C cable",
                    "Black braided cable",
                    "Electronics",
                    2,
                    0.92m,
                    container.Id),
                new(
                    "Sticky notes",
                    "Yellow note pad",
                    "Stationery",
                    1,
                    0.73m,
                    null)
            ],
            createdAt.AddMinutes(2),
            cancellationToken);
    }
}
