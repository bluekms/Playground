using System.Globalization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary;

public sealed record RecordInfo(string SheetName, Type RecordType)
{
    public const string RecordSuffix = "Records";

    public string DbSetName => $"{SheetName}{RecordSuffix}";
}

// TODO RecordFinder가 접미사로 가져오지 말고 상속이나 Attribute를 쓰게할지 고민
public sealed class RecordFinder
{
    public static List<RecordInfo> Find<T>() where T : DbContext
    {
        var myComp = CultureInfo.InvariantCulture.CompareInfo;
        
        var recordPropertyInfoList = typeof(T)
            .GetProperties()
            .Where(x => myComp.IsSuffix(x.Name, RecordInfo.RecordSuffix))
            .ToList();
        
        var list = new List<RecordInfo>(recordPropertyInfoList.Count);
        
        foreach (var pi in recordPropertyInfoList)
        {
            var sheetNameAttribute = pi.GetCustomAttributes()
                .SingleOrDefault(x => x.GetType() == typeof(SheetName));

            var recordName = pi.Name;
            if(sheetNameAttribute != null)
            {
                recordName = (sheetNameAttribute as ColumnName)!.Name;
            }

            var sheetName = recordName.Replace(RecordInfo.RecordSuffix, string.Empty);
            var t = pi.PropertyType.GetGenericArguments()[0];
            list.Add(new(sheetName, t));
        }
        
        return list;
    }

    public static PropertyInfo Find<T>(string name)
    {
        var myComp = CultureInfo.InvariantCulture.CompareInfo;

        return typeof(T)
            .GetProperties()
            .Where(x => myComp.IsSuffix(x.Name, RecordInfo.RecordSuffix))
            .Single(x => x.Name == name);
    }
}