using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

/// <summary>
/// 1 Credential 1 Server (Service)
/// 약간 모호한걸?
/// </summary>
public class Credential
{
    public string Name { get; set; } = null!;

    public string Token { get; set; } = null!;

    public ServerRoles Role { get; set; }

    public string Description { get; set; } = null!;
}

internal sealed class CredentialConfiguration : IEntityTypeConfiguration<Credential>
{
    public void Configure(EntityTypeBuilder<Credential> builder)
    {
        builder.HasKey(e => e.Name);
    }
}
