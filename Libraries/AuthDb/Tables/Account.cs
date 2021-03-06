using System.ComponentModel.DataAnnotations;
using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

/// <summary>
/// 1 Account 1 User
/// </summary>
public sealed class Account
{
    [Key]
    public string AccountId { get; init; } = null!;

    public string Password { get; init; } = null!;

    public DateTime CreatedAt { get; set; }

    public string Token { get; set; } = null!;

    public UserRoles Role { get; set; }
}

internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Account");
    }
}