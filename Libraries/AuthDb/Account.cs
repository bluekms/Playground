using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

public sealed class Account
{
    [AllowNull]
    [Key]
    public string AccountId { get; init; }

    [AllowNull]
    public string Password { get; init; }

    public DateTime CreatedAt { get; set; }

    [AllowNull]
    public string Token { get; set; }

    public UserRoles Role { get; set; }
}

internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(k => new {k.AccountId});
    }
}