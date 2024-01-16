using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

public class Password
{
    public string AccountId { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public string AccountPassword { get; set; } = null!;
}

internal sealed class PasswordConfiguration : IEntityTypeConfiguration<Password>
{
    public void Configure(EntityTypeBuilder<Password> builder)
    {
        builder.HasKey(e => e.AccountId);
        builder.HasOne<Account>()
            .WithOne()
            .HasForeignKey<Account>(e => e.AccountId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
