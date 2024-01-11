using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.DevRecords.TestRecords;

public sealed class ForeignTestRecord
{
    [Key]
    [Order]
    public int Id { get; set; }

    [ForeignKey(nameof(TargetTestRecord), nameof(TargetTestRecord.Id))]
    [Order]
    public int TargetTestId { get; set; }

    [Order]
    public string Value { get; set; } = null!;

    [ForeignKey(nameof(ClassListTestRecord), nameof(ClassListTestRecord.StudentId))]
    [Order]
    public string? StudentId { get; set; }

    [Order]
    [ColumnName("비고")]
    public string? Note { get; set; }
}

internal sealed class ForeignTestRecordConfiguration : IEntityTypeConfiguration<ForeignTestRecord>
{
    public void Configure(EntityTypeBuilder<ForeignTestRecord> builder)
    {
        builder.HasOne<TargetTestRecord>()
            .WithMany()
            .HasForeignKey(x => x.TargetTestId)
            .IsRequired();

        builder.HasOne<ClassListTestRecord>()
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .IsRequired();
    }
}
