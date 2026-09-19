using ItemOrganizer.Domain;
using ItemOrganizer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ItemOrganizer.Infrastructure.Tests;

[Collection(PostgreSqlCollection.Name)]
public sealed class PersistenceTests(PostgreSqlFixture fixture)
{
    private static readonly Guid TenantId =
        Guid.Parse("a0000000-0000-0000-0000-000000000001");
    private static readonly Guid OwnerId =
        Guid.Parse("b0000000-0000-0000-0000-000000000001");
    private static readonly DateTimeOffset Now =
        new(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task Migration_CreatesApprovedTables()
    {
        await using var connection = new NpgsqlConnection(fixture.ConnectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT table_name
            FROM information_schema.tables
            WHERE table_schema = 'public'
            ORDER BY table_name
            """;

        var tables = new List<string>();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tables.Add(reader.GetString(0));
        }

        Assert.Contains("containers", tables);
        Assert.Contains("photos", tables);
        Assert.Contains("analyses", tables);
        Assert.Contains("items", tables);
        Assert.Contains("item_assignments", tables);
        Assert.Contains("outbox_messages", tables);
        Assert.Contains("idempotency_records", tables);
    }

    [Fact]
    public async Task ConcurrentUpdate_RejectsStaleVersion()
    {
        var id = Guid.NewGuid();
        await using (var arrangeContext = fixture.CreateContext())
        {
            arrangeContext.Containers.Add(CreateContainer(id, "Original"));
            await arrangeContext.SaveChangesAsync();
        }

        await using var firstContext = fixture.CreateContext();
        await using var secondContext = fixture.CreateContext();
        var first = await firstContext.Containers.SingleAsync(container => container.Id == id);
        var second = await secondContext.Containers.SingleAsync(container => container.Id == id);

        first.Update("First update", null, null, null, Now.AddMinutes(1));
        await firstContext.SaveChangesAsync();

        second.Update("Stale update", null, null, null, Now.AddMinutes(2));
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => secondContext.SaveChangesAsync());
    }

    [Fact]
    public async Task IdempotencyKey_IsUniquePerOwnerAndOperation()
    {
        await using var dbContext = fixture.CreateContext();
        dbContext.IdempotencyRecords.AddRange(
            CreateIdempotencyRecord(Guid.NewGuid()),
            CreateIdempotencyRecord(Guid.NewGuid()));

        await Assert.ThrowsAsync<DbUpdateException>(
            () => dbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task Assignment_CannotReferenceAnotherOwnersContainer()
    {
        var setup = await CreateRunningAnalysisAsync();
        var foreignContainer = new StorageContainer(
            Guid.NewGuid(),
            TenantId,
            Guid.NewGuid(),
            $"Foreign-{Guid.NewGuid():N}",
            null,
            null,
            null,
            Now);
        var item = new Item(
            Guid.NewGuid(),
            TenantId,
            OwnerId,
            setup.PhotoId,
            setup.AnalysisId,
            "Cable",
            null,
            "Electronics",
            1,
            0.9m,
            Guid.NewGuid().ToString("N"),
            Now.AddMinutes(2));
        item.SetAssignment(ItemAssignment.Suggested(
            Guid.NewGuid(),
            TenantId,
            OwnerId,
            item.Id,
            foreignContainer.Id,
            Now.AddMinutes(2)));

        await using var dbContext = fixture.CreateContext();
        dbContext.AddRange(foreignContainer, item);

        await Assert.ThrowsAsync<DbUpdateException>(
            () => dbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CompletedAnalysis_PersistsMergedItemsAndAssignmentsAtomically()
    {
        var setup = await CreateRunningAnalysisAsync();

        await using (var dbContext = fixture.CreateContext())
        {
            var persistence = new AnalysisResultPersistence(dbContext);
            await persistence.PersistCompletedAnalysisAsync(
                setup.AnalysisId,
                [
                    new("Cable", "First", "Electronics", 1, 0.91m, setup.ContainerId),
                    new(" cable ", "Second", "electronics", 2, 0.95m, setup.ContainerId),
                    new("Unknown", null, null, 1, 0.20m, null)
                ],
                Now.AddMinutes(2));
        }

        await using var assertContext = fixture.CreateContext();
        var analysis = await assertContext.Analyses.SingleAsync(
            candidate => candidate.Id == setup.AnalysisId);
        var item = await assertContext.Items
            .Include(candidate => candidate.Assignment)
            .SingleAsync(candidate => candidate.AnalysisId == setup.AnalysisId);

        Assert.Equal(AnalysisStatus.Completed, analysis.Status);
        Assert.Single(analysis.Warnings);
        Assert.Equal(3, item.Quantity);
        Assert.Equal(0.95m, item.Confidence);
        Assert.Equal(AssignmentStatus.Suggested, item.Assignment.Status);
        Assert.Equal(setup.ContainerId, item.Assignment.SuggestedContainerId);
    }

    [Fact]
    public async Task FailedResultPersistence_RollsBackAnalysisAndItems()
    {
        var setup = await CreateRunningAnalysisAsync();

        await using (var dbContext = fixture.CreateContext())
        {
            var persistence = new AnalysisResultPersistence(dbContext);
            await Assert.ThrowsAsync<DomainException>(
                () => persistence.PersistCompletedAnalysisAsync(
                    setup.AnalysisId,
                    [new("Invalid", null, null, 0, 0.9m, null)],
                    Now.AddMinutes(2)));
        }

        await using var assertContext = fixture.CreateContext();
        var analysis = await assertContext.Analyses.SingleAsync(
            candidate => candidate.Id == setup.AnalysisId);
        Assert.Equal(AnalysisStatus.Running, analysis.Status);
        Assert.False(await assertContext.Items.AnyAsync(
            item => item.AnalysisId == setup.AnalysisId));
    }

    private async Task<(Guid AnalysisId, Guid ContainerId, Guid PhotoId)>
        CreateRunningAnalysisAsync()
    {
        var container = CreateContainer(Guid.NewGuid(), $"Container-{Guid.NewGuid():N}");
        var photo = new Photo(
            Guid.NewGuid(),
            TenantId,
            OwnerId,
            $"photos/{Guid.NewGuid():N}.webp",
            "image/webp",
            1_024,
            512,
            512,
            Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"),
            Now.AddDays(30),
            Now);
        var analysis = new Analysis(
            Guid.NewGuid(),
            TenantId,
            OwnerId,
            photo.Id,
            "prompt-v1",
            "schema-v1",
            "model-v1",
            "tests",
            null,
            Now);
        analysis.Start(Now.AddMinutes(1));

        await using var dbContext = fixture.CreateContext();
        dbContext.AddRange(container, photo, analysis);
        await dbContext.SaveChangesAsync();
        return (analysis.Id, container.Id, photo.Id);
    }

    private static StorageContainer CreateContainer(Guid id, string name)
    {
        return new StorageContainer(
            id,
            TenantId,
            OwnerId,
            name,
            null,
            null,
            null,
            Now);
    }

    private static IdempotencyRecord CreateIdempotencyRecord(Guid id)
    {
        return new IdempotencyRecord(
            id,
            TenantId,
            OwnerId,
            "photo-upload",
            "same-key",
            new string('a', 64),
            Now.AddHours(1),
            Now);
    }
}
