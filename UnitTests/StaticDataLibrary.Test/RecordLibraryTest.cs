using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.Attributes;
using StaticDataLibrary.RecordLibrary;
using StaticDataLibrary.ValidationLibrary;

namespace StaticDataLibrary.Test;

public sealed class RecordLibraryTest : IStaticDataContextTester
{
    private const string TestStaticDataPath = @"../../../../../StaticData/__TestStaticData/Output";

    [Fact]
    public void TableCountTest()
    {
        var tableInfoList = TableFinder.Find<TestStaticDataContext>();
        Assert.True(
            tableInfoList.Count == TestStaticDataContext.TestTableCount,
            $"Check the suffix. {TableInfo.DbSetNameSuffix}");
    }

    [Fact]
    public void MustUseSealedClass()
    {
        var tableInfoList = TableFinder.Find<TestStaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            Assert.True(tableInfo.RecordType.IsSealed, $"{tableInfo.RecordType.Name} must be sealed");
        }
    }

    [Fact]
    public void RequiredAttributeTest()
    {
        var tableInfoList = TableFinder.Find<TestStaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            PropertyAttributeFinder.ValidateSingle<KeyAttribute>(tableInfo);

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
            Assert.True(
                compareInfo.IsSuffix(tableName, TableInfo.RecordTypeNameSuffix),
                $"The suffix is different. {tableName}, {TableInfo.RecordTypeNameSuffix}");
        }
    }

    [Fact]
    public async Task RangeAttributeTestAsync()
    {
        await RangeChecker.CheckAsync<TestStaticDataContext>(TestStaticDataPath);
    }

    [Fact]
    public async Task RegexAttributeTestAsync()
    {
        await RegexChecker.CheckAsync<TestStaticDataContext>(TestStaticDataPath);
    }

    [Fact]
    public async Task InsertSqliteTestAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await using var context = await InitializeStaticData(connection, "InsertTest.db");

        var nameCount = await context.NameTestTable.CountAsync();
        var arrayCount = await context.ArrayTestTable.CountAsync();
        var classCount = await context.ClassListTestTable.CountAsync();
        var complexCount = await context.ComplexTestTable.CountAsync();
        var groupItemCount = await context.GroupedItemTestTable.CountAsync();
        var groupCount = await context.GroupTestTable.CountAsync();

        Assert.Equal(5, nameCount);
        Assert.Equal(5, arrayCount);
        Assert.Equal(3, classCount);
        Assert.Equal(2, complexCount);
        Assert.Equal(5, groupItemCount);
        Assert.Equal(2, groupCount);
    }

    [Fact]
    public async Task ForeignTableCheckTestAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await using var context = await InitializeStaticData(connection, "ForeignTest.db");

        await ForeignChecker.CheckAsync<TestStaticDataContext>(connection);
    }

    [Theory]
    [InlineData("ClassListTestTable", 3, "INSERT INTO ClassListTestTable VALUES (20220003,CCC,영어,D,,,,,\"국어, 수학 미응시\");")]
    [InlineData("ComplexTestTable", 2, "INSERT INTO ComplexTestTable VALUES (1학년2반,20220201,XXX,국어,C,영어,C,수학,C,,20220202,YYY,국어,A,수학,A,,,영어 미응시,20220203,ZZZ,국어,A,영어,A,수학,A,참 잘했어요.,담임 미정);")]
    public async void InsertQueryBuilderTest(string dbSetName, int rowCount, string expected)
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

    // TODO 쿼리빌더 다른 쿼리들도 테스트
    private static async Task<TestStaticDataContext> InitializeStaticData(SqliteConnection connection, string dbFileName)
    {
        var options = new DbContextOptionsBuilder<TestStaticDataContext>()
            .UseSqlite(connection)
            .Options;

        var context = new TestStaticDataContext(options, dbFileName);
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

            await RecordSqlExecutor.InsertAsync(connection, tableInfo, dataList, transaction!);
        }

        await transaction!.CommitAsync();

        return context;
    }
}
