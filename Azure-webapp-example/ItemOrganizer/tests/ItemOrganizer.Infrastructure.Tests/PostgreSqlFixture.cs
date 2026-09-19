using ItemOrganizer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ItemOrganizer.Infrastructure.Tests;

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private string _adminConnectionString = string.Empty;

    public string DatabaseName { get; } =
        $"itemorganizer_test_{Guid.NewGuid():N}";

    public string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        var configuredConnectionString = Environment.GetEnvironmentVariable(
            "ITEMORGANIZER_DATABASE_CONNECTION");
        if (string.IsNullOrWhiteSpace(configuredConnectionString))
        {
            throw new InvalidOperationException(
                "ITEMORGANIZER_DATABASE_CONNECTION is required for integration tests.");
        }

        var testBuilder = new NpgsqlConnectionStringBuilder(configuredConnectionString)
        {
            Database = DatabaseName
        };
        ConnectionString = testBuilder.ConnectionString;

        var adminBuilder = new NpgsqlConnectionStringBuilder(configuredConnectionString)
        {
            Database = "postgres"
        };
        _adminConnectionString = adminBuilder.ConnectionString;

        await using (var adminConnection = new NpgsqlConnection(_adminConnectionString))
        {
            await adminConnection.OpenAsync();
            await using var createCommand = adminConnection.CreateCommand();
            createCommand.CommandText = $"CREATE DATABASE \"{DatabaseName}\"";
            await createCommand.ExecuteNonQueryAsync();
        }

        await using var dbContext = CreateContext();
        await dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await using var adminConnection = new NpgsqlConnection(_adminConnectionString);
        await adminConnection.OpenAsync();
        await using var dropCommand = adminConnection.CreateCommand();
        dropCommand.CommandText = $"DROP DATABASE IF EXISTS \"{DatabaseName}\" WITH (FORCE)";
        await dropCommand.ExecuteNonQueryAsync();
    }

    public ItemOrganizerDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ItemOrganizerDbContext>()
            .UseNpgsql(ConnectionString)
            .UseSnakeCaseNamingConvention()
            .Options;

        return new ItemOrganizerDbContext(options);
    }
}

[CollectionDefinition(Name)]
public sealed class PostgreSqlCollection : ICollectionFixture<PostgreSqlFixture>
{
    public const string Name = "PostgreSQL";
}
