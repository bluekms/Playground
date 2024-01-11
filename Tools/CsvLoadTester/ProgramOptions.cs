using CommandLine;

namespace CsvLoadTester;

public class ProgramOptions
{
    [Option('d', "directory", Required = true, HelpText = "csv 파일들이 있는 디렉토리. file과 함께 사용될수 없습니다.")]
    public string CsvDirectory { get; set; } = null!;

    [Option('t', "test", Required = false, HelpText = "TestStaticDataContext")]
    public bool UseTestContext { get; set; } = false;

    [Option('o', "output", Required = false, HelpText = "발생한 문제를 해당 경로에 txt 파일로 출력합니다.")]
    public required string OutputPath { get; set; }
}
