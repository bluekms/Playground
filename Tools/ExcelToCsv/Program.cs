using CommandLine;
using ExcelToCsv;

const string command = "-f D:\\Workspace\\gitProject\\Playground\\StaticData\\SampleStaticData.xlsx -o D:\\Workspace\\gitProject\\Playground\\StaticData\\Output";

Parser.Default.ParseArguments<ProgramOptions>(command.Split(' '))
    .WithParsed(RunOptions)
    .WithNotParsed(HandleParseError);

static async void RunOptions(ProgramOptions options)
{
    var processor = new ExcelConverter();
    processor.LoadExcelFile(options.FileName, options.SheetName);
    await processor.PrintCsvFilesAsync(options.OutputPath, options.Target);
}

static void HandleParseError(IEnumerable<Error> errors)
{
    Console.WriteLine($"Errors {errors.Count()}");
    foreach (var error in errors)
    {
        Console.WriteLine(error.ToString());
    }
}