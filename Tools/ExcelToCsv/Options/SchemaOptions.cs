using CommandLine;

namespace ExcelToCsv;

public sealed class SchemaOptions
{
    [Option('t', "targets", Required = false, HelpText = "타겟 지정되지 않은 컬럼 및 타겟에 해당하는 컬럼만을 출력")]
    public IEnumerable<string>? Targets { get; set; }

    [Option('k', "key", Required = false, HelpText = "이 테이블에서 유일한 값")]
    public bool IsKey { get; set; } = false;

    [Option('p', "type", Required = false, HelpText = "데이터 타입")]
    public string? Type { get; set; }
    
    [Option('n', "type-name", Required = false, HelpText = "별도의 데이터 타입을 가진다면 그 타입의 이름")]
    public string? TypeName { get; set; }
    
    [Option('c', "class", Required = false, HelpText = "특정 클래스의 맴버라면 그 클래스의 이름")]
    public string? ClassName { get; set; }
    
    [Option('b', "foreign-table", Required = false, HelpText = "다른 테이블을 참조하고 있다면 테이블의 이름")]
    public string? ForeignTable { get; set; }
    
    [Option('y', "foreign-key", Required = false, HelpText = "다른 테이블을 참조하고 있다면 테이블의 Key 컬럼 이름")]
    public string? ForeignKey { get; set; }
}