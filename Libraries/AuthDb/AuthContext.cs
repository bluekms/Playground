using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace AuthDb
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

        [AllowNull] public DbSet<Account> Accounts { get; set; }
        [AllowNull] public DbSet<Maintenance> Maintenance { get; set; }
        [AllowNull] public DbSet<World> Worlds { get; set; }
        [AllowNull] public DbSet<Foo> Foos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(k => new { k.AccountId });
            modelBuilder.Entity<Maintenance>().HasKey(k => new { k.Start });
            modelBuilder.Entity<World>().HasKey(k => new { k.WorldName });
            modelBuilder.Entity<Foo>().HasKey(k => new { k.Seq });
        }
    }
}