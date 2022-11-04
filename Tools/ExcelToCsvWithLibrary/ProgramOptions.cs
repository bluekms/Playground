using CommandLine;

namespace ExcelToCsvWithFile;

public sealed class ProgramOptions
{
    [Option('e', "excel", Required = false, HelpText = "단 하나의 액셀 파일. directory와 함께 사용할수 없습니다.")]
    public string? ExcelFile { get; set; }
    
    [Option('s', "sheet", Required = false, HelpText = "액셀 파일 하나의 특정 시트를 지정. file과 함께 사용될수 있습니다. directory라면 무시됩니다.")]
    public string? SheetName { get; set; }
    
    [Option('d', "directory", Required = false, HelpText = "액셀 파일들이 있는 디렉토리. file과 함께 사용될수 없습니다.")]
    public string? ExcelDirectory { get; set; }

    [Option('o', "output", Required = true, HelpText = "csv 파일 출력 경로")]
    public string OutputPath { get; set; } = null!;

    [Option('t', "test", Required = false, HelpText = "TestStaticDataContext")]
    public bool UseTestContext { get; set; } = false;
}