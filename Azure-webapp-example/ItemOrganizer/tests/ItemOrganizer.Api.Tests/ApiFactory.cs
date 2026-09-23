using ItemOrganizer.Domain;
using ItemOrganizer.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ItemOrganizer.Api.Tests;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName =
        $"item-organizer-api-{Guid.NewGuid():N}";

    public static readonly Guid TenantId =
        Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid OwnerId =
        Guid.Parse("20000000-0000-0000-0000-000000000001");
    public static readonly Guid OtherOwnerId =
        Guid.Parse("20000000-0000-0000-0000-000000000002");
    public static readonly Guid ContainerId =
        Guid.Parse("30000000-0000-0000-0000-000000000001");
    public static readonly Guid OtherContainerId =
        Guid.Parse("30000000-0000-0000-0000-000000000002");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting(
            "ITEMORGANIZER_DATABASE_CONNECTION",
            "Host=localhost;Database=itemorganizer_api_tests");
        builder.UseSetting("Authentication:AllowedTenantId", TenantId.ToString());
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ItemOrganizerDbContext>>();
            services.RemoveAll<
                IDbContextOptionsConfiguration<ItemOrganizerDbContext>>();
            services.RemoveAll<ItemOrganizerDbContext>();
            services.AddDbContext<ItemOrganizerDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        TestAuthenticationHandler.SchemeName;
                    options.DefaultChallengeScheme =
                        TestAuthenticationHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                    TestAuthenticationHandler.SchemeName,
                    _ => { });

            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            Seed(scope.ServiceProvider.GetRequiredService<ItemOrganizerDbContext>());
        });
    }

    private static void Seed(ItemOrganizerDbContext dbContext)
    {
        var createdAt = new DateTimeOffset(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);
        var container = new StorageContainer(
            ContainerId,
            TenantId,
            OwnerId,
            "Office supplies",
            "Primary container",
            "Home office",
            ["office"],
            createdAt);
        var secondContainer = new StorageContainer(
            Guid.Parse("30000000-0000-0000-0000-000000000003"),
            TenantId,
            OwnerId,
            "Cables",
            null,
            "Garage",
            [],
            createdAt.AddMinutes(1));
        var otherContainer = new StorageContainer(
            OtherContainerId,
            TenantId,
            OtherOwnerId,
            "Private",
            null,
            null,
            [],
            createdAt);
        var photo = new Photo(
            Guid.Parse("40000000-0000-0000-0000-000000000001"),
            TenantId,
            OwnerId,
            "private/photo.webp",
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
            OwnerId,
            photo.Id,
            "prompt-v1",
            "schema-v1",
            "mock",
            "tests",
            null,
            createdAt);
        analysis.Start(createdAt.AddSeconds(1));
        analysis.Complete([], createdAt.AddSeconds(2));
        var item = new Item(
            Guid.Parse("60000000-0000-0000-0000-000000000001"),
            TenantId,
            OwnerId,
            photo.Id,
            analysis.Id,
            "USB-C cable",
            "Black cable",
            "Electronics",
            1,
            0.95m,
            "usb-c",
            createdAt);
        var assignment = ItemAssignment.Confirmed(
            Guid.Parse("70000000-0000-0000-0000-000000000001"),
            TenantId,
            OwnerId,
            item.Id,
            container.Id,
            AssignmentSource.User,
            createdAt);
        item.SetAssignment(assignment);

        dbContext.AddRange(
            container,
            secondContainer,
            otherContainer,
            photo,
            analysis,
            item);
        dbContext.SaveChanges();
    }
}
