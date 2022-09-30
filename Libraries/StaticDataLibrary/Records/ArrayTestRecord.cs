using System.ComponentModel.DataAnnotations;

namespace StaticDataLibrary.Records;

public class ArrayTestRecord
{
    [Key]
    public int Id { get; set; }
    public int? Value1 { get; set; }
    public int? Value2 { get; set; }
    public int? Value3 { get; set; }
    public int? Value4 { get; set; }
    public int? Value5 { get; set; }
    public string? Info { get; set; }
}