using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.Attributes;
using StaticDataLibrary.RecordLibrary;
using StaticDataLibrary.ValidationLibrary;

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
            var finder = new PropertyAttributeFinder();
            PropertyAttributeFinder.ValidateSingle<KeyAttribute>(tableInfo);

            var propertyCount = tableInfo.RecordType.GetProperties().Length;
            var orderCount = PropertyAttributeFinder.Count<OrderAttribute>(tableInfo);
            Assert.True(orderCount == propertyCount, "Record의 모든 컬럼에 [Order] Attribute가 필요합니다.");
        }
    }

    [Fact]
    public void MustUseSealedClass()
    {
        var tableInfoList = TableFinder.Find<StaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            Assert.True(tableInfo.RecordType.IsSealed, $"{tableInfo.RecordType.Name} must be sealed");
        }
    }

    [Fact]
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
            Assert.True(
                compareInfo.IsSuffix(tableName, TableInfo.RecordTypeNameSuffix),
                $"The suffix is different. {tableName}, {TableInfo.RecordTypeNameSuffix}");
        }
    }

    [Fact]
    public async Task RangeAttributeTestAsync()
    {
        await RangeChecker.CheckAsync<StaticDataContext>(RealStaticDataPath);
    }

    [Fact]
    public async Task RegexAttributeTestAsync()
    {
        await RegexChecker.CheckAsync<StaticDataContext>(RealStaticDataPath);
    }

    [Fact]
    public async Task InsertSqliteTestAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await using var context = await InitializeStaticData(connection, "RealInsertTest.db");
    }

    [Fact]
    public async Task ForeignTableCheckTestAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await using var context = await InitializeStaticData(connection, "RealForeignTest.db");

        await ForeignChecker.CheckAsync<StaticDataContext>(connection);
    }

    private static async Task<StaticDataContext> InitializeStaticData(SqliteConnection connection, string dbFileName)
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
