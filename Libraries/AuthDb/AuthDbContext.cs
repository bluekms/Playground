using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace AuthDb
{
    public sealed class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<Foo> Foos => Set<Foo>();

        public DbSet<Account> Accounts => Set<Account>();

        public DbSet<Maintenance> Maintenance => Set<Maintenance>();

        public DbSet<Server> Servers => Set<Server>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}