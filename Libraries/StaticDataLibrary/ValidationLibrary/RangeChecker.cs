using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.ValidationLibrary;

public static class RangeChecker
{
    public static async Task CheckAsync<T>(string staticDataPath) where T : DbContext
    {
        var tableInfoList = TableFinder.Find<T>();
        foreach (var tableInfo in tableInfoList)
        {
            var fileName = Path.Combine(
                Directory.GetCurrentDirectory(),
                staticDataPath,
                $"{tableInfo.SheetName}.csv");
            
            var properties = OrderedPropertySelector.GetList(tableInfo.RecordType);
            
            var dataList = await RecordParser.GetDataListAsync(tableInfo, fileName);
            foreach (var data in dataList)
            {
                foreach (var propertyInfo in properties)
                {
                    var attribute = PropertyAttributeFinder.Find<RangeAttribute>(tableInfo, propertyInfo.Name);
                    if (attribute == null)
                    {
                        continue;
                    }

                    var value = data.GetType()
                        .GetProperty(propertyInfo.Name)!
                        .GetValue(data, null) ?? null;

                    if (value == null)
                    {
                        if (propertyInfo.IsNullable())
                        {
                            continue;    
                        }
                        else
                        {
                            throw new ArgumentNullException($"Nullable Error. {tableInfo.RecordType.Name}.{propertyInfo.Name} 반드시 값이 있어야 합니다.");
                        }
                    }
                    
                    if (!attribute.IsValid(value))
                    {
                        if (value.GetType() == typeof(DateTime))
                        {
                            var val = (DateTime)value;
                            var min = (DateTime)attribute.Minimum;
                            var max = (DateTime)attribute.Maximum;
                            throw new ValidationException($"Range Error. {tableInfo.RecordType.Name}.{propertyInfo.Name}({val.ToString("s")}) 반드시 {min.ToString("s")} ~ {max.ToString("s")} 사이여야 합니다.");    
                        }
                        else
                        {
                            throw new ValidationException($"Range Error. {tableInfo.RecordType.Name}.{propertyInfo.Name}({value}) 반드시 {attribute.Minimum} ~ {attribute.Maximum} 사이여야 합니다.");
                        }
                    }
                } // for propertyInfo
            } // for data
        } // for table
    }
}