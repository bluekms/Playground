using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorldDb.Tables;

public class WorldFoo
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public long Seq { get; set; }

    [Column(TypeName = "char")]
    [MaxLength(50)]
    public string Data { get; set; } = null!;
}

internal sealed class WorldFooConfiguration : IEntityTypeConfiguration<WorldFoo>
{
    public void Configure(EntityTypeBuilder<WorldFoo> builder)
    {
        builder.ToTable("Foo");
    }
}
