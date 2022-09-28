using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.Records;

[SheetName("이름 매핑 테스트")]
public sealed record NameMappingTestRecord
{
    public int Id { get; init; }
    
    public int Value1 { get; set; }
    
    [ColumnName("값2")]
    public int Value2 { get; set; }
}