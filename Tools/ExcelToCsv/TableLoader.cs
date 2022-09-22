using System.Text;
using ExcelDataReader;
using StaticDataLibrary;

namespace ExcelToCsv;

public class TableLoader
{
    private static char SkipToken = '_';
    
    private readonly List<ColumnSchema> columnSchemaList = new();
    private readonly List<List<string?>> rowList = new();
    
    public TableLoader(IExcelDataReader reader, string sheetName, int startColumn)
    {
        while (reader.Read())
        {
            if (columnSchemaList.Count == 0)
            {
                ReadHeader(reader, startColumn);    
            }

            reader.Read();
            
            ReadBody(reader, startColumn);
        }

        Name = sheetName;
    }
    
    public string Name { get; init; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var schema in columnSchemaList)
        {
            sb.AppendLine($" - {schema}");
        }

        sb.AppendLine();
        
        foreach (var row in rowList)
        {
            sb.AppendJoin(',', row);
        }
        return sb.ToString();
    }

    public HashSet<string> GetColumnTargets()
    {
        var result = new HashSet<string>();
        foreach (var schema in columnSchemaList)
        {
            foreach (var target in schema.SchemaData!.OutputTargets)
            {
                result.Add(target);    
            }
        }

        result.Remove(SchemaData.TargetAll);

        return result;
    }

    public List<List<string?>> GetOutputData(string target)
    {
        var targetIndexList = columnSchemaList
            .Where(x => x.SchemaData!.OutputTargets.Contains(SchemaData.TargetAll)
                        || x.SchemaData!.OutputTargets.Contains(target))
            .Select(x => x.ColumnIndex)
            .ToList();

        var result = new List<List<string?>>(rowList.Count);
        foreach (var row in rowList)
        {
            var targetColumns = new List<string?>();
            foreach (var index in targetIndexList)
            {
                targetColumns.Add(row[index]);
            }
            
            result.Add(targetColumns);
        }

        return result;
    }
    
    private void ReadHeader(IExcelDataReader reader, int startColumn)
    {
        for (var i = startColumn; i < reader.FieldCount; ++i)
        {
            columnSchemaList.Add(new ColumnSchema(i - startColumn, reader.GetString(i)));
        }
        
        reader.Read();

        var index = startColumn;
        foreach (var schema in columnSchemaList)
        {
            var source = reader.GetString(index++);
            schema.SetSourceName(source);
        }
        
        columnSchemaList.RemoveAll(x => x.SourceName != null && x.SourceName[0] == SkipToken);

        InitializeHeader();
    }

    private void ReadBody(IExcelDataReader reader, int startColumn)
    {
        var cells = new List<string?>(reader.FieldCount - startColumn);
        for (int i = startColumn; i < reader.FieldCount; ++i)
        {
            var str = reader.GetValue(i)?.ToString() ?? null;

            cells.Add(CsvUtility.ToCsv(str));
        }

        rowList.Add(cells);
    }

    private void InitializeHeader()
    {
        foreach (var columnSchema in columnSchemaList)
        {
            if (columnSchema.SchemaData != null)
            {
                continue;
            }

            var parser = new ContainerNameParser(columnSchema.SourceName!);
            if (!parser.IsContainerItem)
            {
                throw new InvalidOperationException($"{columnSchema.SourceName}은 배열이 아닙니다. 스키마가 비어있을 수 없습니다.");   
            }

            if (parser.ContainerIndex == 0)
            {
                throw new InvalidOperationException($"{columnSchema.SourceName}은 배열의 첫 번째 컬럼입니다. 스키마가 비어있을 수 없습니다.");   
            }

            var sameFirstItem = columnSchemaList
                .Where(x => x.SchemaData != null)
                .Where(x => x.SchemaData!.IsContainerItem)
                .Where(x => x.SchemaData!.ContainerIndex == 0)
                .Single(x => x.SchemaData!.Name!.Equals(parser.PureName));

            columnSchema.SetSchema(sameFirstItem.SourceSchema!);
            columnSchema.SchemaData?.SetName(columnSchema.SourceName!);
        }
    }
}