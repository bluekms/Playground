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
    }
    // TODO Library로 빼야 하나?
    public sealed record Account(string AccountId, string Password, string SessionId, DateTime CreatedAt, string Authority);
}