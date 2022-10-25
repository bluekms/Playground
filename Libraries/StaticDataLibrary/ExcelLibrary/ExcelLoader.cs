using System.Diagnostics;
using ExcelDataReader;

namespace StaticDataLibrary.ExcelLibrary;

public sealed class ExcelLoader
{
 public List<SheetLoader> SheetList { get; } = new();

    public ExcelLoader(string fileName, string? sheetName)
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
            if (!string.IsNullOrWhiteSpace(sheetName))
            {
                if (!reader.Name.Equals(sheetName))
                {
                    continue;
                }    
            }
            
            SheetLoader sheet;

            var sw = new Stopwatch();
            sw.Start();

            try
            {
                var startColumn = MoveToFirstCell(reader);
                sheet = new SheetLoader(reader, reader.Name, startColumn);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Load Failure:\t{excelFileName}.{reader.Name}. {e.Message}");
                continue;
            }
            
            sw.Stop();
            Console.WriteLine($"Load Complete:\t{excelFileName}.{sheet.Name} ({sw.Elapsed.TotalMilliseconds} ms)");
            
            SheetList.Add(sheet);
            
        } while (reader.NextResult());
    }
    
    private int MoveToFirstCell(IExcelDataReader reader)
    {
        reader.Read();
        var a1 = reader.GetString(0);
        if (string.IsNullOrWhiteSpace(a1))
        {
            throw new FormatException("각 시트의 a1에는 반드시 데이터 해더의 셀 이름이 있어야 합니다. (ex. B5)");
        }

        try
        {
            var cellLocation = new CellLocationParser(a1);
            
            for (var i = 0; i < cellLocation.RowNumber - 2; ++i)
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