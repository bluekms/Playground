using CommandLine;

namespace ExcelToCsv;

internal sealed class SchemaOptions
{
    [Option('t', "target", Required = false, HelpText = "타겟 지정되지 않은 컬럼 및 타겟에 해당하는 컬럼만을 출력")]
    public string Target { get; init; } = null!;
    
    [Option('k', "Key", Required = false, HelpText = "이 테이블 내에서 고유한 키")]
    public bool IsKey { get; init; }
}