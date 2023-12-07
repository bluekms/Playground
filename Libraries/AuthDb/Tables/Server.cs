using System.ComponentModel.DataAnnotations;
using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

/// <summary>
/// 서버의 정보를 담고있는 테이블
/// </summary>
public sealed class Server
{
    [Key]
    public string Name { get; init; } = null!;

    public ServerRoles Role { get; set; }

    public string Address { get; set; } = null!;

    public DateTime ExpireAt { get; set; }

    public string Description { get; set; } = null!;
}

internal sealed class ServerConfiguration : IEntityTypeConfiguration<Server>
{
    public void Configure(EntityTypeBuilder<Server> builder)
    {
        builder.ToTable("Server");
    }
}
