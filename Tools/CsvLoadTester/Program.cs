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

await Parser.Default.ParseArguments<ProgramOptions>(args)
    .WithParsedAsync(RunOptionsAsync);

static async Task RunOptionsAsync(ProgramOptions programOptions)
{
    var swTotal = new Stopwatch();
    swTotal.Start();
    
    try
    {
        var sw = new Stopwatch();
        sw.Start();
        Console.WriteLine("InitializeStaticData ...");
        var connection = new SqliteConnection("DataSource=:memory:");
        var options = new DbContextOptionsBuilder<TestStaticDataContext>().UseSqlite(connection).Options;
        var context = new TestStaticDataContext(options, "StaticData.db");
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        await connection.OpenAsync();
        await InitializeStaticData(context, connection, programOptions.CsvDirectory);
        sw.Stop();
        Console.WriteLine($"InitializeStaticData Fin: {sw.Elapsed.TotalMilliseconds}ms");
        
        var errors = new List<string>();
        
        sw.Reset();
        sw.Start();
        Console.WriteLine("Range Check ...");
        await RangeChecker.CheckAsync<TestStaticDataContext>(programOptions.CsvDirectory, errors);
        sw.Stop();
        Console.WriteLine($"Range Check Fin: {sw.Elapsed.TotalMilliseconds}ms");
        
        sw.Reset();
        sw.Start();
        Console.WriteLine("Regex Check ...");
        await RegexChecker.CheckAsync<TestStaticDataContext>(programOptions.CsvDirectory, errors);
        sw.Stop();
        Console.WriteLine($"Regex Check Fin: {sw.Elapsed.TotalMilliseconds}ms");
        
        sw.Reset();
        sw.Start();
        Console.WriteLine("Foreign Check ...");
        await ForeignChecker.CheckAsync<TestStaticDataContext>(connection, errors);
        sw.Stop();
        Console.WriteLine($"Regex Check Fin: {sw.Elapsed.TotalMilliseconds}ms");
        
        if (errors.Count > 0)
        {
            if (!string.IsNullOrWhiteSpace(programOptions.OutputPath))
            {
                File.WriteAllLines(programOptions.OutputPath, errors, Encoding.UTF8);
            }

            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }

    swTotal.Stop();
    Console.WriteLine($"Fin:\t{swTotal.Elapsed.TotalMilliseconds}ms");
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