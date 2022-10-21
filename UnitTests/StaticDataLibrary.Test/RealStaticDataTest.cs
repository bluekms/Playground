using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.Attributes;
using StaticDataLibrary.DevRecords;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.Test;

public sealed class RealStaticDataTest : IStaticDataContextTester
{
    // TODO 경로 수정
    private const string RealStaticDataPath = @"../../../../../StaticData/__TestStaticData/Output";

    [Fact]
    public void RequiredAttributeTest()
    {
        var tableInfoList = TableFinder.Find<StaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            PropertyAttributeFinder.Single<KeyAttribute>(tableInfo);

            var propertyCount = tableInfo.RecordType.GetProperties().Length;
            var orderCount = PropertyAttributeFinder.Count<OrderAttribute>(tableInfo);
            Assert.True(orderCount == propertyCount, "Record의 모든 컬럼에 [Order] Attribute가 필요합니다.");
        }
    }

    public async Task LoadCsvToRecordTestAsync()
    {
        var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        
        var tableInfoList = TableFinder.Find<StaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            var fileName = Path.Combine(
                Directory.GetCurrentDirectory(),
                RealStaticDataPath,
                $"{tableInfo.SheetName}.csv");

            var dataList = await RecordParser.GetDataListAsync(tableInfo, fileName);
            Assert.NotEmpty(dataList);

            var tableName = dataList[0]?.GetType().Name ?? string.Empty;
            Assert.True(compareInfo.IsSuffix(tableName, TableInfo.TypeNameSuffix), 
                $"The suffix is different. {tableName}, {TableInfo.TypeNameSuffix}");
        }
    }

    [Fact]
    public async Task RangeAttributeTestAsync()
    {
        var tableInfoList = TableFinder.Find<StaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            var fileName = Path.Combine(
                Directory.GetCurrentDirectory(),
                RealStaticDataPath,
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

    [Fact]
    public async Task InsertSqliteTestAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        var options = new DbContextOptionsBuilder<StaticDataContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new StaticDataContext(options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync() as SqliteTransaction;

        var tableInfoList = TableFinder.Find<StaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            var fileName = Path.Combine(RealStaticDataPath, $"{tableInfo.SheetName}.csv");
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }
            
            var dataList = await RecordParser.GetDataListAsync(tableInfo, fileName);

            await RecordDataInserter.InsertAsync(tableInfo.RecordType, tableInfo.DbSetName, dataList, connection, transaction!);
        }
        
        await transaction!.CommitAsync();
    }
}