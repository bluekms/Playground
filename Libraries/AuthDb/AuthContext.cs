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
        [AllowNull] public DbSet<Credential> Credentials { get; set; }
        [AllowNull] public DbSet<Maintenance> Maintenance { get; set; }
        [AllowNull] public DbSet<Server> Servers { get; set; }
        [AllowNull] public DbSet<Foo> Foos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(k => new { k.AccountId });
            modelBuilder.Entity<Credential>().HasKey(k => new { k.Name });
            modelBuilder.Entity<Maintenance>().HasKey(k => new { k.Id });
            modelBuilder.Entity<Server>().HasKey(k => new { k.Name });
            modelBuilder.Entity<Foo>().HasKey(k => new { k.Seq });
        }
    }
}