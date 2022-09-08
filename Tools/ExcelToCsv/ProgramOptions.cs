using CommandLine;

namespace ExcelToCsv;

internal sealed class ProgramOptions
{
    [Option('f', "file", Required = true, HelpText = "엑셀 파일 경로")]
    public string FileName { get; init; } = null!;

    [Option('o', "output", Required = true, HelpText = "출력 파일 경로")]
    public string OutputPath { get; init; } = null!;
    
    [Option('t', "target", Required = false, HelpText = "타겟이 명시된 컬럼이 있다면 이 인자값과 같을 경우에만 출력에 포함 됨")]
    public string? Target { get; set; }
}