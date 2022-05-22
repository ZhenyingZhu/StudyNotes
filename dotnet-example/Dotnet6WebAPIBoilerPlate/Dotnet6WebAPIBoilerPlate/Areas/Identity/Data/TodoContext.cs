using Dotnet6WebAPIBoilerPlate.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Dotnet6WebAPIBoilerPlate.Data;

namespace Dotnet6WebAPIBoilerPlate.Data;

public class TodoContext : IdentityDbContext<TodoUser>
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Dotnet6WebAPIBoilerPlate.Data.TodoItem> TodoItem { get; set; }
}
