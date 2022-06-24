using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

/// <summary>
/// 서버 점검
/// </summary>
public class Maintenance
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public long Id { get; set; }

    public DateTime Start { get; init; }

    public DateTime End { get; init; }

    public string Reason { get; init; } = null!;

    public override string ToString()
    {
        return $"Id: {Id}, Start: {Start}, End: {End}, Reason: {Reason}";
    }
}

internal sealed class MaintenanceConfiguration : IEntityTypeConfiguration<Maintenance>
{
    public void Configure(EntityTypeBuilder<Maintenance> builder)
    {
        builder.ToTable("Maintenance");
    }
}