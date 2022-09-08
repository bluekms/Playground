using System.Data;
using System.Xml;
using ExcelDataReader;

namespace ExcelToCsv;

public sealed class ExcelConverter
{
    private static char SkipToken = '_';

    public void ReadFileStream(string fileName)
    {
        using var loader = new LockedFileStreamLoader(fileName);
        if (loader.IsTemp)
        {
            var lastWriteTime = File.GetLastWriteTime(fileName);
            Console.WriteLine($"이미 열려있어 사본을 읽습니다. 파일: {fileName}. 마지막으로 저장된 시간: {lastWriteTime}");
        }

        using var reader = ExcelReaderFactory.CreateReader(loader.Stream);
        do
        {
            if (reader.Name[0] == SkipToken)
            {
                Console.WriteLine($"{reader.Name} is Skip");
            }
            else
            {
                var startColumn = MoveToFirstCell(reader);
                
                var schemaList = new List<StaticDataSchema>(); 
                while (reader.Read())
                {
                    if (schemaList.Count == 0)
                    {
                        GatherSchema(reader, startColumn, schemaList);
                    }
                    
                    for (int i = startColumn; i < reader.FieldCount; ++i)
                    {
                        Console.Write($"{reader.GetValue(i)}, ");
                    }
                    Console.WriteLine();
                }    
            }
        } while (reader.NextResult());
    }

    private int MoveToFirstCell(IDataReader reader)
    {
        reader.Read();
        var a1 = reader.GetString(0);
        var cellLocation = new CellLocation(a1);
        
        for (int i = 0; i < cellLocation.RowNumber - 2; ++i)
        {
            reader.Read();
        }

        return cellLocation.ColumnNumber - 1;
    }

    private void GatherSchema(IDataReader reader, int startColumn, List<StaticDataSchema> schemaList)
    {
        for (int i = startColumn; i < reader.FieldCount; ++i)
        {
            schemaList.Add(new StaticDataSchema(reader.GetString(i)));
        }

        reader.Read();

        var index = startColumn;
        foreach (var schema in schemaList)
        {
            schema.SetName(reader.GetString(index++));
        }
    }

    private void LoadData(IDataReader reader, List<int> foo)
    {
    }

    private void WriteCsvFile()
    {
    }
}