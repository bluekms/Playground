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
        [AllowNull] public DbSet<World> Worlds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(k => new { k.AccountId });
            modelBuilder.Entity<World>().HasKey(k => new { k.WorldName });
        }
    }
}