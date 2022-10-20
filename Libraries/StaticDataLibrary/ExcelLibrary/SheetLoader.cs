using System.Text;
using ExcelDataReader;

namespace StaticDataLibrary.ExcelLibrary;

public sealed class SheetLoader
{
    private readonly List<string> columnNameList = new();
    private readonly List<List<string?>> rowList = new();
    private readonly List<int> targetColumnIndices = new();
    
    public SheetLoader(IExcelDataReader reader, string sheetName, int startColumn)
    {
        while (reader.Read())
        {
            if (columnNameList.Count == 0)
            {
                ReadHeader(reader, startColumn);
                reader.Read();
            }
            ReadBody(reader, startColumn);
        }

        Name = sheetName;
    }

    public string Name { get; }

    public List<string> ToCsvLineList(IList<string>? targetNameList = null)
    {
        SetColumnIndices(targetNameList);
        
        var list = new List<string>(rowList.Count);
        
        var sb = new StringBuilder();
        foreach (var row in rowList)
        {
            sb.Clear();
            for (var i = 0; i < targetColumnIndices.Count; ++i)
            {
                var index = targetColumnIndices[i];
                var column = row[index];
                sb.Append(column);
                if (i < targetColumnIndices.Count - 1)
                {
                    sb.Append(',');
                }
            }
            
            list.Add(sb.ToString());
        }

        return list;
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
            
            cells.Add(QuotationMarks.Wrapped(str));
        }
        
        rowList.Add(cells);
    }

    private void SetColumnIndices(IList<string>? targetNameList)
    {
        targetColumnIndices.Clear();

        if (targetNameList == null)
        {
            SetAllColumnIndices();
            return;
        }

        foreach (var name in targetNameList)
        {
            var index = columnNameList.IndexOf(name);
            if (index < 0)
            {
                throw new IndexOutOfRangeException($"Not found column. {name}");
            }
            
            targetColumnIndices.Add(index);
        }
    }

    private void SetAllColumnIndices()
    {
        for (var i = 0; i < columnNameList.Count; ++i)
        {
            targetColumnIndices.Add(i);
        }
    }
}