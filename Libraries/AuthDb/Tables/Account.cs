using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

/// <summary>
/// 1 Account 1 User
/// </summary>
public sealed class Account
{
    public string AccountId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string Token { get; set; } = null!;

    public ResSignUp.Types.AccountRoles Role { get; set; }
}

internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(e => e.AccountId);
    }
}
