using System.Diagnostics;
using ExcelDataReader;
using StaticDataLibrary.ExcelLibrary.Exceptions;

namespace StaticDataLibrary.ExcelLibrary;

// TODO https://github.com/MarkPflug/Sylvan 조사해보기
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
                if (!reader.Name.Equals(sheetName, StringComparison.OrdinalIgnoreCase))
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
        }
        while (reader.NextResult());
    }

    private static int MoveToFirstCell(IExcelDataReader reader)
    {
        reader.Read();
        var a1 = reader.GetString(0);
        if (string.IsNullOrWhiteSpace(a1))
        {
            throw new A1CellException(a1);
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
            throw new A1CellException(a1, e);
        }
    }
}
