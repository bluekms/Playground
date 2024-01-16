using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace AuthDb;

public sealed class AuthDbContext : DbContext, IAuthDbContext
{
    public const string ConfigurationSection = "AuthDb";

    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<Foo> Foos => Set<Foo>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Password> Passwords => Set<Password>();
    public DbSet<Maintenance> Maintenances => Set<Maintenance>();
    public DbSet<ServerRole> ServerRoles => Set<ServerRole>();
    public DbSet<Server> Servers => Set<Server>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasDefaultSchema(ConfigurationSection);

        // modelBuilder.HasPostgresExtension("pg_bigm");
    }
}
