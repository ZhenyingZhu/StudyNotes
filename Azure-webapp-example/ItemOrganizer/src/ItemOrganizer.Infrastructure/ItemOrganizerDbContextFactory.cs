using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ItemOrganizer.Infrastructure;

public sealed class ItemOrganizerDbContextFactory : IDesignTimeDbContextFactory<ItemOrganizerDbContext>
{
    public ItemOrganizerDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable(
            "ITEMORGANIZER_DATABASE_CONNECTION");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "ITEMORGANIZER_DATABASE_CONNECTION is required.");
        }

        var options = new DbContextOptionsBuilder<ItemOrganizerDbContext>()
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .Options;

        return new ItemOrganizerDbContext(options);
    }
}
