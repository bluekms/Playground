using System.ComponentModel.DataAnnotations;

namespace StaticDataLibrary.Records;

public sealed record TargetTest
{
    [Key]
    public int Id { get; init; }
    public int Value1 { get; set; }
    public int Value3 { get; set; }
}