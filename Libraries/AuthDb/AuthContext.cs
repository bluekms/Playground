using System;
using Microsoft.EntityFrameworkCore;

namespace AuthDb
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(k => new { k.AccountId });
        }

        public sealed class Account
        {
            public string? AccountId { get; set; }
            public string? Password { get; set; }
            public string? SessionId { get; set; }
            public DateTime CreatedAt { get; set; }
            public string? Authority { get; set; }
        }
    }
}