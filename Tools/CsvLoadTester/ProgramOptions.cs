using CommandLine;

namespace CsvLoadTester;

public class ProgramOptions
{
    [Option('d', "directory", Required = true, HelpText = "csv 파일들이 있는 디렉토리. file과 함께 사용될수 없습니다.")]
    public string CsvDirectory { get; set; } = null!;
}