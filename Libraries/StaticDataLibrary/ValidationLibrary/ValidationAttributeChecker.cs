using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.ValidationLibrary;

public class ValidationAttributeChecker<TDbContext, TAttribute>
    where TDbContext : DbContext
    where TAttribute : ValidationAttribute
{
    public delegate void ValidationFailureHandler(TAttribute attribute, object value, string location);

    public async Task CheckAsync(string staticDataPath, ValidationFailureHandler handler)
    {
        var tableInfoList = TableFinder.Find<TDbContext>();
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
                    var attribute = PropertyAttributeFinder.Find<TAttribute>(tableInfo, propertyInfo.Name);
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
                        handler(attribute, value, $"{tableInfo.RecordType.Name}.{propertyInfo.Name}");
                    }
                } // for propertyInfo
            } // for data
        } // for table
    }
}
