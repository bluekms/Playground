using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.DevRecords.TestRecords;

public sealed class GroupedItemTestRecord
{
    [Key]
    [Order]
    public string Id { get; set; } = null!;
    
    [ForeignKey(nameof(GroupTestRecord), nameof(GroupTestRecord.GroupId))]
    [Order]
    public string GroupId { get; set; } = null!;
    
    [Order]
    public int Value { get; set; }
}

public sealed class GroupedItemTestRecordConfiguration : IEntityTypeConfiguration<GroupedItemTestRecord>
{
    public void Configure(EntityTypeBuilder<GroupedItemTestRecord> builder)
    {
        builder.HasIndex(x => x.GroupId);
        
        // HasOne/HasMany, WithOne/WithMany는 사용하지 않는다
        // StaticData에서는 CRUD에 분명한 제한이 생기는 외례키는 없고
        // 별도의 테스트 코드에서 외례키의 존재 유무를 확인한다
        //builder.HasOne<GroupTestRecord>()
        //    .WithMany()
        //    .HasForeignKey(x => x.GroupId)
        //    .IsRequired();
    }
}