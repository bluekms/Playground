using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace StaticDataLibrary.Records;

public sealed class TargetTestRecord
{
    [Key]
    [Index(0)]
    public int Id { get; set; }
    
    [Index(1)]
    public int Value1 { get; set; }
    
    [Index(2)]
    public int Value3 { get; set; }
}