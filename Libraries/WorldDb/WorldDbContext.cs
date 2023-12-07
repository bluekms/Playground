using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WorldDb.Tables;

namespace WorldDb;

public class WorldDbContext : DbContext
{
    public const string ConfigurationSection = "WorldDb";

    public WorldDbContext(DbContextOptions<WorldDbContext> options)
        : base(options)
    {
    }

    public DbSet<WorldFoo> Foos => Set<WorldFoo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasDefaultSchema(ConfigurationSection);
    }
}
