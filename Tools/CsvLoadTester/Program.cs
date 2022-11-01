using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using CommandLine;
using CsvLoadTester;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.RecordLibrary;
using StaticDataLibrary.Test;
using StaticDataLibrary.ValidationLibrary;

// TODO 실 사용 시 TestStaticDataContext 를 StaticDataContext 로 교체해야 함

Parser.Default.ParseArguments<ProgramOptions>(args)
    .WithParsedAsync(RunOptionsAsync);

static async Task RunOptionsAsync(ProgramOptions programOptions)
{
    var connection = new SqliteConnection("DataSource=:memory:");
    
    var sw = new Stopwatch();
    sw.Start();
    
    try
    {
        await RangeChecker.CheckAsync<TestStaticDataContext>(programOptions.CsvDirectory);
        await RegexChecker.CheckAsync<TestStaticDataContext>(programOptions.CsvDirectory);

        await using var context = await InitializeStaticData(connection, "StaticData.db", programOptions.CsvDirectory);
        await ForeignChecker.CheckAsync<TestStaticDataContext>(connection);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}

static async Task<TestStaticDataContext> InitializeStaticData(SqliteConnection connection, string dbFileName, string csvFilePath)
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
        var fileName = Path.Combine(csvFilePath, $"{tableInfo.SheetName}.csv");
        if (!File.Exists(fileName))
        {
            Console.WriteLine($"{fileName} 파일이 없습니다. 일단 넘어갑니다.");
        }
            
        var dataList = await RecordParser.GetDataListAsync(tableInfo, fileName);

        await RecordSqlExecutor.InsertAsync(connection, tableInfo, dataList, transaction!);
    }
        
    await transaction!.CommitAsync();

    return context;
}