using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.Records;

[SheetName("이름 매핑 테스트")]
public sealed class NameTestRecord
{
    [Key]
    [Order]
    public int Id { get; set; }
    
    [ColumnName("값2")]
    [Order]
    public int Value2 { get; set; }
    
    [Order]
    public int Value3 { get; set; }
}