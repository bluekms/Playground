using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.Records;

[SheetName("ArrayTest")]
public sealed record ArrayTestRecord
{
    [Key]
    public int Id { get; init; }
    
    [ColumnName("Value")]
    public List<int> ValueList { get; init; } = new();
    
    public string? Info { get; set; }
}