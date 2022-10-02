using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.Records;

public class ArrayTestRecord
{
    [Key]
    [Order]
    public int Id { get; set; }
    
    [Order]
    public int? Value1 { get; set; }
    
    [Order]
    public int? Value2 { get; set; }
    
    [Order]
    public int? Value3 { get; set; }
    
    [Order]
    public int? Value4 { get; set; }
    
    [Order]
    public int? Value5 { get; set; }
    
    [Order]
    public string? Info { get; set; }
}