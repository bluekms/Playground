using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.Attributes;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.Test;

public sealed class StaticDataTest : IStaticDataContextTester
{
    // TODO 실제 개발에 사용할 때는 경로 수정
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
        await using var context = await InitializeStaticData(connection, "RealInsertTest.db");
    }

    [Fact]
    public async Task ForeignTableTestAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await using var context = await InitializeStaticData(connection, "RealForeignTest.db");
        
        var tableInfoList = TableFinder.FindAllTablesWithForeignKey<TestStaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            Assert.NotNull(tableInfo.ForeignInfoList);

            if (tableInfo.ForeignInfoList == null)
            {
                throw new ArgumentNullException(nameof(tableInfo.ForeignInfoList));
            }

            foreach (var foreignInfo in tableInfo.ForeignInfoList)
            {
                var resultList = await RecordSqlExecutor.CheckForeignKey(connection, tableInfo, foreignInfo);

                var sb = new StringBuilder();
                if (resultList.Count > 0)
                {
                    foreach (var result in resultList)
                    {
                        sb.AppendLine(result.ToString());
                    }
                }

                Assert.True(resultList.Count == 0, sb.ToString());
            }
        }
    }
    
    private async Task<StaticDataContext> InitializeStaticData(SqliteConnection connection, string dbFileName)
    {
        var options = new DbContextOptionsBuilder<StaticDataContext>()
            .UseSqlite(connection)
            .Options;

        var context = new StaticDataContext(options, dbFileName);
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

            await RecordSqlExecutor.InsertAsync(connection, tableInfo, dataList, transaction!);
        }
        
        await transaction!.CommitAsync();

        return context;
    }
}