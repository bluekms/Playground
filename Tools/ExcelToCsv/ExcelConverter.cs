using System.Diagnostics;
using ExcelDataReader;

namespace ExcelToCsv;

public sealed class ExcelConverter
{
    private static char SkipToken = '_';
    private readonly List<TableLoader> tables = new();
    
    public void LoadExcelFile(string fileName, string? targetSheet)
    {
        using var loader = new LockedFileStreamLoader(fileName);
        if (loader.IsTemp)
        {
            var lastWriteTime = File.GetLastWriteTime(fileName);
            
            Console.WriteLine($"{Path.GetFileName(fileName)} 이미 열려있어 사본을 읽습니다. 마지막으로 저장된 시간: {lastWriteTime}");
        }

        var excelFileName = Path.GetFileName(fileName);
        
        using var reader = ExcelReaderFactory.CreateReader(loader.Stream);
        do
        {
            if (targetSheet is null)
            {
                if (reader.Name[0] == SkipToken)
                {
                    continue;
                }
            }
            else
            {
                if (!reader.Name.Equals(targetSheet, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
            }
                
            TableLoader table;
                
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                var startColumn = MoveToFirstCell(reader);
                table = new TableLoader(reader, reader.Name, startColumn);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{excelFileName}.{reader.Name} load failure. Elapsed: {sw.Elapsed}. Message: {e.Message}. InnerMessage: {e?.InnerException?.Message ?? "null"}");
                continue;
            }
                
            sw.Stop();
            Console.WriteLine($"{excelFileName}.{table.Name} load complete. Elapsed: {sw.Elapsed}.");
                
            tables.Add(table);
            
        } while (reader.NextResult());
    }

    public async Task PrintCsvFilesAsync(string outputPath, string? target = null)
    {
        foreach (var table in tables)
        {
            TablePrinter printer;
            
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                printer = new TablePrinter(table);
                
                await printer.DoPrintAsync(outputPath, target);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{table.Name} write failure. Elapsed: {sw.Elapsed}. Message: {e.Message}. InnerMessage: {e?.InnerException?.Message ?? "null"}");
                continue;
            }
            
            sw.Stop();
            Console.WriteLine($"{table.Name} write complete. Elapsed: {sw.Elapsed}. Path: {printer.OutputFileName}");
        }
    }

    private int MoveToFirstCell(IExcelDataReader reader)
    {
        reader.Read();
        var a1 = reader.GetString(0);

        try
        {
            var cellLocation = new CellLocationParser(a1);
            
            for (int i = 0; i < cellLocation.RowNumber - 2; ++i)
            {
                reader.Read();
            }

            return cellLocation.ColumnNumber - 1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"{reader.Name}:A1의 값은 반드시 시작 Cell의 이름이어야 합니다. (ex. B10 / 현재: {a1})", e);
        }
    }
}