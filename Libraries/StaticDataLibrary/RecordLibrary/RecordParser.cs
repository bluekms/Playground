using System.Collections;
using System.Text;

namespace StaticDataLibrary.RecordLibrary;

public static class RecordParser
{
    public static async Task<IList> GetDataList(TableInfo tableInfo, string fileName)
    {
        var list = CreateList(tableInfo.RecordType);

        using var reader = new StreamReader(fileName, Encoding.UTF8);
        while (!reader.EndOfStream)
        {
            var csvLine = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(csvLine))
            {
                break;
            }

            var record = RecordMapper.Map(tableInfo.RecordType, csvLine);
            list.Add(record);
        }

        return list;
    }
    
    private static IList CreateList(Type t)
    {
        var values = Array.CreateInstance(t, 0);
        var genericListType = typeof(List<>);
        var concreteListType = genericListType.MakeGenericType(t);
        
        return (Activator.CreateInstance(concreteListType, values) as IList)!;
    }
}