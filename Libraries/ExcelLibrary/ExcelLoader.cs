using System.Diagnostics;
using ExcelDataReader;
using StaticDataLibrary;

namespace ExcelLibrary;

public class ExcelLoader
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
                    // Console.WriteLine($"{reader.Name} sheet skip. Is not the {sheetName}");
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
                Console.WriteLine($"{excelFileName}.{reader.Name} load failure. Elapsed: {sw.Elapsed}. Message: {e.Message}. InnerMessage: {e?.InnerException?.Message ?? "null"}");
                continue;
            }
            
            sw.Stop();
            Console.WriteLine($"{excelFileName}.{sheet.Name} load complete. Elapsed: {sw.Elapsed}.");
            
            SheetList.Add(sheet);
            
        } while (reader.NextResult());
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