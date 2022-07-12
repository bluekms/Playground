using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

public sealed class Foo
{
    [Key]
    public long Seq { get; set; }

    [AllowNull]
    public string AccountId { get; set; }

    [AllowNull]
    public string Command { get; set; }

    public int Value { get; set; }

    public enum FooCommand
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Squared,
        Merge,
    }
}

internal sealed class FooConfiguration : IEntityTypeConfiguration<Foo>
{
    public void Configure(EntityTypeBuilder<Foo> builder)
    {
        builder.ToTable("Foo");
        builder.HasKey(k => new {k.Seq});
    }
}