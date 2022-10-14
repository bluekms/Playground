using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.Attributes;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.Test;

public static class StaticDataContextTester<T> where T : DbContext
{
    public static void RequiredAttributeTest()
    {
        var tableInfoList = TableFinder.Find<T>();
        foreach (var tableInfo in tableInfoList)
        {
            PropertyAttributeFinder.Single<KeyAttribute>(tableInfo);

            var propertyCount = tableInfo.RecordType.GetProperties().Length;
            var orderCount = PropertyAttributeFinder.Count<OrderAttribute>(tableInfo);
            Assert.True(orderCount == propertyCount, "Record의 모든 컬럼에 [Order] Attribute가 필요합니다.");
        }
    }
    
    public static async Task LoadCsvTest(string staticDataPath)
    {
        var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        
        var tableInfoList = TableFinder.Find<T>();
        foreach (var tableInfo in tableInfoList)
        {
            var fileName = Path.Combine(
                Directory.GetCurrentDirectory(),
                staticDataPath,
                $"{tableInfo.SheetName}.csv");

            var dataList = await RecordParser.GetDataList(tableInfo, fileName);
            Assert.NotEmpty(dataList);

            var tableName = dataList[0]?.GetType().Name ?? string.Empty;
            Assert.True(compareInfo.IsSuffix(tableName, TableInfo.TypeNameSuffix), 
                $"The suffix is different. {tableName}, {TableInfo.TypeNameSuffix}");
        }
    }
    
    public static async Task RangeAttributeTest(string staticDataPath)
    {
        var tableInfoList = TableFinder.Find<T>();
        foreach (var tableInfo in tableInfoList)
        {
            var fileName = Path.Combine(
                Directory.GetCurrentDirectory(),
                staticDataPath,
                $"{tableInfo.SheetName}.csv");
            
            var properties = OrderedPropertySelector.GetList(tableInfo.RecordType);
            
            var dataList = await RecordParser.GetDataList(tableInfo, fileName);
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
                            throw new ArgumentNullException($"[{tableInfo.RecordType.Name}].[{propertyInfo.Name}] is not Nullable. but value is null");
                        }
                    }
                    
                    if (!attribute.IsValid(value))
                    {
                        throw new ValidationException($"[{tableInfo.RecordType.Name}].[{propertyInfo.Name}]({value}) must be between {attribute.Minimum} and {attribute.Maximum}");
                    }
                } // for propertyInfo
            } // for data
        } // for table
    }
}