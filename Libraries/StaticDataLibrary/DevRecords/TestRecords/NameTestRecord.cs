using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.DevRecords.TestRecords;

[SheetName("이름 매핑 테스트")]
public class NameTestRecord
{
    [Key]
    [Order]
    public int Id { get; set; }
    
    [ColumnName("값2")]
    [Order]
    public int Value2 { get; set; }
    
    [Order]
    [Range(0, 20)]
    public int Value3 { get; set; }
}