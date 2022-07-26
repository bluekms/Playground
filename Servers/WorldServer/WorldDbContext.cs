using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WorldServer.Tables;

namespace WorldServer;

public sealed class WorldDbContext : DbContext
{
    public WorldDbContext(DbContextOptions<WorldDbContext> options)
        : base(options)
    {
    }

    public DbSet<WorldFoo> Foos => Set<WorldFoo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}