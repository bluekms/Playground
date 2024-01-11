using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.DevRecords.TestRecords;

public sealed class TargetTestRecord
{
    [Key]
    [Order]
    public int Id { get; set; }

    [Order]
    [Range(0, 20)]
    public int Value3 { get; set; }

    [Order]
    public int Value1 { get; set; }
}
