using System.Text;
using ExcelDataReader;

namespace StaticDataLibrary;

public class SheetLoader
{
    private readonly List<string> columnNameList = new();
    private readonly List<List<string?>> rowList = new();
    
    public SheetLoader(IExcelDataReader reader, string sheetName, int startColumn)
    {
        while (reader.Read())
        {
            if (columnNameList.Count == 0)
            {
                ReadHeader(reader, startColumn);
            }

            reader.Read();
            
            ReadBody(reader, startColumn);
        }

        Name = sheetName;
    }

    public string Name { get; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendJoin(',', columnNameList);
        sb.AppendLine();
        foreach (var row in rowList)
        {
            sb.AppendJoin(',', row);
        }

        return sb.ToString();
    }

    private void ReadHeader(IExcelDataReader reader, int startColumn)
    {
        for (var i = startColumn; i < reader.FieldCount; ++i)
        {
            columnNameList.Add(reader.GetString(i));
        }
    }

    private void ReadBody(IExcelDataReader reader, int startColumn)
    {
        var cells = new List<string?>(reader.FieldCount - startColumn);
        for (var i = startColumn; i < reader.FieldCount; ++i)
        {
            var str = reader.GetValue(i)?.ToString() ?? null;
            
            cells.Add(CsvUtility.ToCsv(str));
        }
        
        rowList.Add(cells);
    }
}