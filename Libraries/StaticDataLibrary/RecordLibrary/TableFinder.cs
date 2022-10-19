using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace StaticDataLibrary.RecordLibrary;

public static class TableFinder
{
    public static List<TableInfo> Find<T>() where T : DbContext
    {
        var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        
        var recordPropertyInfoList = typeof(T)
            .GetProperties()
            .Where(x => compareInfo.IsSuffix(x.Name, TableInfo.DbSetNameSuffix))
            .Where(x => compareInfo.IsSuffix(x.PropertyType.GetGenericArguments().First().Name, TableInfo.TypeNameSuffix))
            .ToList();
        
        var list = new List<TableInfo>(recordPropertyInfoList.Count);
        
        foreach (var pi in recordPropertyInfoList)
        {
            list.Add(new(pi));
        }
        
        return list;
    }
}