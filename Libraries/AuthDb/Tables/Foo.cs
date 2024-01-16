using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

/// <summary>
/// For Dev
/// </summary>
public sealed class Foo
{
    public enum FooCommand
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Squared,
        Merge,
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Seq { get; set; }

    public string AccountId { get; set; } = null!;

    public string Command { get; set; } = null!;

    public int Value { get; set; }
}

internal sealed class FooConfiguration : IEntityTypeConfiguration<Foo>
{
    public void Configure(EntityTypeBuilder<Foo> builder)
    {
        builder.HasKey(e => e.Seq);
    }
}
