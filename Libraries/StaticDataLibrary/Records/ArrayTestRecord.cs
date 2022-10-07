using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace StaticDataLibrary.Records;

public class ArrayTestRecord
{
    [Key]
    [Index(1)]
    public int Id { get; set; }
    
    [Index(2)]
    public int? Value1 { get; set; }
    
    [Index(3)]
    public int? Value2 { get; set; }
    
    [Index(4)]
    public int? Value3 { get; set; }
    
    [Index(5)]
    public int? Value4 { get; set; }
    
    [Index(6)]
    public int? Value5 { get; set; }
    
    [Index(7)]
    public string? Info { get; set; }
}