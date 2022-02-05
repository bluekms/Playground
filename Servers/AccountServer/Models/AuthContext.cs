using System;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Models
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

        public sealed record Account(string AccountId, string Password, string SessionId, DateTime CreatedAt, string Authority);
    }
}