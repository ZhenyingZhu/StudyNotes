using ItemOrganizer.Infrastructure;
using Microsoft.EntityFrameworkCore;

var command = args.FirstOrDefault()?.ToLowerInvariant() ?? "migrate";
var connectionString = Environment.GetEnvironmentVariable(
    "ITEMORGANIZER_DATABASE_CONNECTION");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("ITEMORGANIZER_DATABASE_CONNECTION is required.");
    return 1;
}

var options = new DbContextOptionsBuilder<ItemOrganizerDbContext>()
    .UseNpgsql(connectionString)
    .UseSnakeCaseNamingConvention()
    .Options;

await using var dbContext = new ItemOrganizerDbContext(options);

switch (command)
{
    case "migrate":
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("Database migrations applied.");
        return 0;

    case "reset-and-seed":
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
        await DatabaseSeeder.SeedAsync(dbContext);
        Console.WriteLine("Database reset and deterministic seed completed.");
        return 0;

    default:
        Console.Error.WriteLine(
            "Unknown command. Supported commands: migrate, reset-and-seed.");
        return 2;
}
