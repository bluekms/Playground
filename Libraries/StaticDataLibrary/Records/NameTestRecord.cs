using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.Records;

[SheetName("이름 매핑 테스트")]
public class NameTestRecord
{
    [Key]
    [Index(0)]
    public int Id { get; set; }
    
    [Name("값2")]
    [Index(1)]
    public int Value2 { get; set; }
    
    [Index(2)]
    public int Value3 { get; set; }
}