using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.Attributes;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.Test;

public sealed class RecordLibraryTest : IStaticDataContextTester
{
    private const string TestStaticDataPath = @"../../../../../StaticData/__TestStaticData/Output";

    [Fact]
    public void TableCountTest()
    {
        var tableInfoList = TableFinder.Find<TestStaticDataContext>();
        Assert.True(tableInfoList.Count == TestStaticDataContext.TestTableCount, 
            $"Check the suffix. {TableInfo.DbSetNameSuffix}");
    }

    [Fact]
    public void RequiredAttributeTest()
    {
        var tableInfoList = TableFinder.Find<TestStaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            PropertyAttributeFinder.Single<KeyAttribute>(tableInfo);

            var propertyCount = tableInfo.RecordType.GetProperties().Length;
            var orderCount = PropertyAttributeFinder.Count<OrderAttribute>(tableInfo);
            Assert.True(orderCount == propertyCount, "Record의 모든 컬럼에 [Order] Attribute가 필요합니다.");
        }
    }

    [Fact]
    public async Task LoadCsvToRecordTestAsync()
    {
        var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        
        var tableInfoList = TableFinder.Find<TestStaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            var fileName = Path.Combine(
                Directory.GetCurrentDirectory(),
                TestStaticDataPath,
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
        var tableInfoList = TableFinder.Find<TestStaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            var fileName = Path.Combine(
                Directory.GetCurrentDirectory(),
                TestStaticDataPath,
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
                            throw new ArgumentNullException($"{tableInfo.RecordType.Name}.{propertyInfo.Name} is not Nullable. but value is null");
                        }
                    }
                    
                    if (!attribute.IsValid(value))
                    {
                        throw new ValidationException($"{tableInfo.RecordType.Name}.{propertyInfo.Name}({value}) must be between {attribute.Minimum} and {attribute.Maximum}");
                    }
                } // for propertyInfo
            } // for data
        } // for table
    }

    [Fact]
    public async Task InsertSqliteTestAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        var options = new DbContextOptionsBuilder<TestStaticDataContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new TestStaticDataContext(options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync() as SqliteTransaction;

        var tableInfoList = TableFinder.Find<TestStaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            var fileName = Path.Combine(TestStaticDataPath, $"{tableInfo.SheetName}.csv");
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }
            
            var dataList = await RecordParser.GetDataListAsync(tableInfo, fileName);

            await RecordDataInserter.InsertAsync(tableInfo.RecordType, tableInfo.DbSetName, dataList, connection, transaction!);
        }
        
        await transaction!.CommitAsync();
        
        var targetCount = await context.TargetTestTable.CountAsync();
        var nameCount = await context.NameTestTable.CountAsync();
        var arrayCount = await context.ArrayTestTable.CountAsync();
        var classCount = await context.ClassListTestTable.CountAsync();
        var complexCount = await context.ComplexTestTable.CountAsync();
        
        Assert.Equal(5, targetCount);
        Assert.Equal(5, nameCount);
        Assert.Equal(5, arrayCount);
        Assert.Equal(3, classCount);
        Assert.Equal(2, complexCount);
    }

    [Theory]
    [InlineData("TargetTestTable", 5, "INSERT INTO TargetTestTable VALUES (104,19,9);")]
    [InlineData("NameTestTable", 5, "INSERT INTO NameTestTable VALUES (104,10,19);")]
    [InlineData("ClassListTestTable", 3, "INSERT INTO ClassListTestTable VALUES (20220003,CCC,영어,D,,,,,\"국어, 수학 미응시\");")]
    [InlineData("ComplexTestTable", 2, "INSERT INTO ComplexTestTable VALUES (1학년2반,20220201,XXX,국어,C,영어,C,수학,C,,20220202,YYY,국어,A,수학,A,,,영어 미응시,20220203,ZZZ,국어,A,영어,A,수학,A,참 잘했어요.,담임 미정);")]
    public async void RecordQueryBuilderTest(string dbSetName, int rowCount, string expected)
    {
        var tableInfo = TableFinder
            .Find<TestStaticDataContext>()
            .Single(x => x.DbSetName == dbSetName);
        
        var fileName = Path.Combine(
            Directory.GetCurrentDirectory(),
            TestStaticDataPath,
            $"{tableInfo.SheetName}.csv");
        
        var query = RecordQueryBuilder.InsertQuery(tableInfo, out var parameters);

        // 대체로 가장 마지막 데이터가 가장 독특한 형태
        var dataList = await RecordParser.GetDataListAsync(tableInfo, fileName);
        var lastData = dataList[^1]!;
        
        var propertiesCount = lastData.GetType().GetProperties().Length;
        var parametersCount = parameters?.Count ?? 0;
        Assert.Equal(propertiesCount, parametersCount);
        
        foreach (var name in parameters!)
        {
            var value = lastData.GetType()
                .GetProperty(name)!
                .GetValue(lastData, null) ?? DBNull.Value;

            query = query.Replace($"@{name}", value.ToString());
        }
        
        Assert.Equal(dataList.Count, rowCount);
        Assert.Equal(expected, query);
    }
    
    
}