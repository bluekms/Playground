using CommandLine;

namespace ExcelToCsvUsingRecords;

public class ProgramOptions
{
    [Option('e', "excel", Required = true, HelpText = "엑셀 파일 경로")]
    public string ExcelFileName { get; set; } = null!;
    
    [Option('s', "sheet", Required = false, HelpText = "시트 명")]
    public string? SheetName { get; set; }

    [Option('r', "records", Required = false, HelpText = "Client Records 파일 경로 (서버는 이미 알고있음)")]
    public string? RecordsPath { get; set; }
    
    [Option('o', "output", Required = true, HelpText = "출력 파일 경로")]
    public string OutputPath { get; set; } = null!;
}