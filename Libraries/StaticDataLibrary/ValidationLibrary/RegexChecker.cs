using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.ValidationLibrary;

public static class RegexChecker
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
                    var attribute = PropertyAttributeFinder.Find<RegularExpressionAttribute>(tableInfo, propertyInfo.Name);
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
                        throw new ValidationException($"Regex Error. {tableInfo.RecordType.Name}.{propertyInfo.Name}({value}) 패턴이 맞지 않습니다.");
                    }
                } // for propertyInfo
            } // for data
        } // for table
    }
}