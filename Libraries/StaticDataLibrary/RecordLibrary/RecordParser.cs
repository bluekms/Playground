using System.Collections;
using System.Text;

namespace StaticDataLibrary.RecordLibrary;

public static class RecordParser
{
    public static async Task<IList> GetDataList(RecordInfo recordInfo, string fileName)
    {
        var list = CreateList(recordInfo.RecordType);

        using var reader = new StreamReader(fileName, Encoding.UTF8);
        while (!reader.EndOfStream)
        {
            var csvLine = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(csvLine))
            {
                break;
            }

            var record = RecordMapper.Map(recordInfo.RecordType, csvLine);
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