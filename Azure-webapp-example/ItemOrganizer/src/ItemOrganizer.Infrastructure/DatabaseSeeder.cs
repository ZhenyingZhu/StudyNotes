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
            [],
            createdAt.AddMinutes(2),
            cancellationToken);

        var detections = await dbContext.AnalysisDetections
            .Where(detection => detection.AnalysisId == analysis.Id)
            .OrderBy(detection => detection.CreatedAt)
            .ThenBy(detection => detection.Id)
            .ToListAsync(cancellationToken);
        foreach (var detection in detections)
        {
            var item = new Item(
                Guid.NewGuid(),
                TenantId,
                OwnerObjectId,
                photo.Id,
                analysis.Id,
                detection.Name,
                detection.Description,
                detection.Category,
                detection.Quantity,
                detection.Confidence,
                $"detection:{detection.Id:N}",
                createdAt.AddMinutes(3));
            var assignment = detection.SuggestedContainerId is Guid containerId
                ? ItemAssignment.Confirmed(
                    Guid.NewGuid(),
                    TenantId,
                    OwnerObjectId,
                    item.Id,
                    containerId,
                    AssignmentSource.User,
                    createdAt.AddMinutes(3))
                : ItemAssignment.Unassigned(
                    Guid.NewGuid(),
                    TenantId,
                    OwnerObjectId,
                    item.Id,
                    createdAt.AddMinutes(3));
            item.SetAssignment(assignment);
            detection.Accept(
                detection.Name,
                detection.Description,
                detection.Category,
                detection.Quantity,
                assignment.ContainerId,
                item.Id,
                createdAt.AddMinutes(3));
            dbContext.Items.Add(item);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
