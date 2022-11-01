using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
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
    var sw = new Stopwatch();
    sw.Start();
    
    try
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        var options = new DbContextOptionsBuilder<TestStaticDataContext>().UseSqlite(connection).Options;
        var context = new TestStaticDataContext(options, "StaticData.db");
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        await connection.OpenAsync();
        await InitializeStaticData(context, connection, programOptions.CsvDirectory);
        
        var errors = new StringBuilder();
        await RangeChecker.CheckAsync<TestStaticDataContext>(programOptions.CsvDirectory, errors);
        await RegexChecker.CheckAsync<TestStaticDataContext>(programOptions.CsvDirectory, errors);
        await ForeignChecker.CheckAsync<TestStaticDataContext>(connection, errors);
        if (errors.Length > 0)
        {
            throw new ValidationException(errors.ToString());
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }

    sw.Stop();
    Console.WriteLine($"Csv Read Test Success. {sw.Elapsed.TotalMilliseconds}ms");
}

static async Task InitializeStaticData<T>(T context, SqliteConnection connection, string csvFilePath) where T : DbContext
{
    await using var transaction = await connection.BeginTransactionAsync() as SqliteTransaction;

    var tableInfoList = TableFinder.Find<T>();
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
}