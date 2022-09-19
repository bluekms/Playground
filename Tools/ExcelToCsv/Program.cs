using CommandLine;
using ExcelToCsv;

var args2 = new List<string>();
args2.Add("-f");
args2.Add(@"D:\Workspace\gitProject\Playground\StaticData\SampleStaticData.xlsx");
args2.Add("-o");
args2.Add(@"D:\Workspace\gitProject\Playground\StaticData\Output");

Parser.Default.ParseArguments<ProgramOptions>(args2)
    .WithParsed(RunOptions)
    .WithNotParsed(HandleParseError);

static void RunOptions(ProgramOptions options)
{
    var processor = new ExcelConverter();
    processor.ReadFileStream(options.FileName);
}

static void HandleParseError(IEnumerable<Error> errors)
{
    Console.WriteLine($"Errors {errors.Count()}");
    foreach (var error in errors)
    {
        Console.WriteLine(error.ToString());
    }
}