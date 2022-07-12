using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

public class Credential
{
    [AllowNull]
    [Key]
    public string Name { get; set; }

    [AllowNull]
    public string Token { get; set; }

    public ServerRoles Role { get; set; }

    [AllowNull]
    public string Description { get; set; }
}

internal sealed class CredentialConfiguration : IEntityTypeConfiguration<Credential>
{
    public void Configure(EntityTypeBuilder<Credential> builder)
    {
        builder.ToTable("Credential");
        builder.HasKey(k => new {k.Name});
    }
}