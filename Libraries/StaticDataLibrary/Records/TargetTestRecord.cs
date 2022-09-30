using System.ComponentModel.DataAnnotations;

namespace StaticDataLibrary.Records;

public sealed class TargetTestRecord
{
    [Key]
    public int Id { get; set; }
    public int Value1 { get; set; }
    public int Value3 { get; set; }
}