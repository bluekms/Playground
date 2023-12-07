using System.ComponentModel.DataAnnotations;
using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

public sealed class ServerRole
{
    [Key]
    public string Token { get; init; } = null!;

    public ServerRoles Role { get; set; }

    public string Description { get; set; } = null!;
}

internal sealed class ServerRoleConfiguration : IEntityTypeConfiguration<ServerRole>
{
    public void Configure(EntityTypeBuilder<ServerRole> builder)
    {
        builder.ToTable("ServerRole");
    }
}
