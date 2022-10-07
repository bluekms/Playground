using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace StaticDataLibrary;

public static class RecordFinder
{
    public static List<RecordInfo> Find<T>() where T : DbContext
    {
        var myComp = CultureInfo.InvariantCulture.CompareInfo;
        
        var recordPropertyInfoList = typeof(T)
            .GetProperties()
            .Where(x => myComp.IsSuffix(x.Name, RecordInfo.DbSetNameSuffix))
            .ToList();
        
        var list = new List<RecordInfo>(recordPropertyInfoList.Count);
        
        foreach (var pi in recordPropertyInfoList)
        {
            list.Add(new(pi));
        }
        
        return list;
    }
}