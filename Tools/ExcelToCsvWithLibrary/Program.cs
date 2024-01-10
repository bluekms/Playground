using System.Diagnostics;
using System.Text;
using CommandLine;
using ExcelToCsvWithFile;
using StaticDataLibrary;
using StaticDataLibrary.ExcelLibrary;
using StaticDataLibrary.RecordLibrary;
using StaticDataLibrary.Test;

Parser.Default.ParseArguments<ProgramOptions>(args)
    .WithParsed(RunOptions)
    .WithNotParsed(HandleParseError);

static void RunOptions(ProgramOptions options)
{
    if (string.IsNullOrWhiteSpace(options.ExcelFile) &&
        string.IsNullOrWhiteSpace(options.ExcelDirectory))
    {
        var msg = "엑셀 파일명(-e) 혹은 액셀 파일들의 폴더명(-d)은 반드시 입력되어야 합니다. --help를 참조하세요.";
        Console.WriteLine(msg);
        throw new ArgumentException(msg);
    }

    var di = new DirectoryInfo(options.OutputPath);
    if (!di.Exists)
    {
        di.Create();
    }

    var sw = new Stopwatch();
    sw.Start();

    if (!string.IsNullOrWhiteSpace(options.ExcelFile))
    {
        RunExcelToCsv(options.UseTestContext, options.OutputPath, options.ExcelFile, options.SheetName);
    }
    else
    {
        var xlsFiles = Directory.GetFiles(options.ExcelDirectory!, "*.xls");
        var xlsxFiles = Directory.GetFiles(options.ExcelDirectory!, "*.xlsx");

        var list = new List<string>(xlsFiles.Length + xlsxFiles.Length);
        list.AddRange(xlsFiles);
        list.AddRange(xlsxFiles);

        var skipList = list
            .Where(x => Path.GetFileName(x)[0] == '_')
            .ToList();

        foreach (var excel in skipList)
        {
            Console.WriteLine($"Skip: {excel}");
        }

        var targetList = list
            .Where(x => Path.GetFileName(x)[0] != '_')
            .ToList();

        foreach (var excel in targetList)
        {
            RunExcelToCsv(options.UseTestContext, options.OutputPath, excel);
        }
    }

    sw.Stop();
    Console.WriteLine($"Fin:\t{sw.Elapsed.TotalMilliseconds}ms");
}

static void HandleParseError(IEnumerable<Error> errors)
{
    Console.WriteLine($"Errors {errors.Count()}");
    foreach (var error in errors)
    {
        Console.WriteLine(error.ToString());
    }
}

static void RunExcelToCsv(bool useTestContext, string outputPath, string excelFileName, string? sheetName = null)
{
    try
    {
        var loader = new ExcelLoader(excelFileName, sheetName);
        Console.WriteLine($"Load Success:\t{excelFileName}");

        var sw = new Stopwatch();
        foreach (var sheet in loader.SheetList)
        {
            sw.Reset();
            sw.Start();

            var excelSheetName = $"{Path.GetFileNameWithoutExtension(excelFileName)}.{sheet.Name}";

            var tableInfo = useTestContext
                ? TableFinder.Find<TestStaticDataContext>(sheet.Name)
                : TableFinder.Find<StaticDataContext>(sheet.Name);

            if (tableInfo == null)
            {
                Console.WriteLine($"Skip:\t{excelSheetName}");
                continue;
            }

            var csvLines = sheet.ToCsvLineList(tableInfo.ColumnNameList);
            var fileName = Path.Combine(outputPath, $"{tableInfo.SheetName}.csv");

            File.WriteAllLines(fileName, csvLines, Encoding.UTF8);

            sw.Stop();
            Console.WriteLine($"Write Complete:\t{excelSheetName} ({sw.Elapsed.TotalMilliseconds} ms)");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}
