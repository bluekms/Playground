using System.Text;
using CommandLine;
using ExcelToCsvWithFile;
using StaticDataLibrary.DevRecords;
using StaticDataLibrary.ExcelLibrary;
using StaticDataLibrary.RecordLibrary;

Parser.Default.ParseArguments<ProgramOptions>(args)
    .WithParsed(RunOptions)
    .WithNotParsed(HandleParseError);

static void RunOptions(ProgramOptions options)
{
    if (string.IsNullOrWhiteSpace(options.ExcelFile) &&
        string.IsNullOrWhiteSpace(options.ExcelDirectory))
    {
        Console.WriteLine("엑셀 파일명(-e) 혹은 액셀 파일들의 폴더명(-d)은 반드시 입력되어야 합니다. --help를 참조하세요.");
        throw new ArgumentException();
    }
    
    var di = new DirectoryInfo(options.OutputPath);
    if (!di.Exists)
    {
        di.Create();
    }

    if (!string.IsNullOrWhiteSpace(options.ExcelFile))
    {
        RunExcelToCsv(options.OutputPath, options.ExcelFile, options.SheetName);
    }
    else
    {
        var xlsFiles = Directory.GetFiles(options.ExcelDirectory!, "*.xls");
        var xlsxFiles = Directory.GetFiles(options.ExcelDirectory!, "*.xlsx");
        
        var list = new List<string>(xlsFiles.Length + xlsxFiles.Length);
        list.AddRange(xlsFiles);
        list.AddRange(xlsxFiles);

        var targetList = list
            .Where(x => Path.GetFileName(x)[0] != '_')
            .ToList();

        foreach (var excel in targetList)
        {
            RunExcelToCsv(options.OutputPath, excel);
        }
    }
}

static void HandleParseError(IEnumerable<Error> errors)
{
    Console.WriteLine($"Errors {errors.Count()}");
    foreach (var error in errors)
    {
        Console.WriteLine(error.ToString());
    }
}

static void RunExcelToCsv(string outputPath, string excelFileName, string? sheetName = null)
{
    try
    {
        var loader = new ExcelLoader(excelFileName, sheetName);
    
        foreach (var sheet in loader.SheetList)
        {
            var tableInfo = TableFinder.Find<StaticDataContext>(sheet.Name);
            var csvLines = sheet.ToCsvLineList(tableInfo.ColumnNameList);
            var fileName = Path.Combine(outputPath, $"{tableInfo.SheetName}.csv");
        
            File.WriteAllLines(fileName, csvLines, Encoding.UTF8);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}