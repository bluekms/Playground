using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StaticDataLibrary.DevRecords;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.Extensions;

public static class StaticDataExtension
{
    private static string StaticDataPath = "StaticData";
    private const string Extension = ".csv";
    
    public static async void UseStaticData(this IServiceCollection services)
    {
        services.AddEntityFrameworkSqlite().AddDbContext<StaticDataContext>();
        
        await using var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<StaticDataContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        
        await using SqliteConnection connection = new SqliteConnection(context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync() as SqliteTransaction;
        
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var tableInfoList = TableFinder.Find<StaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            var fileName = Path.Combine(path, StaticDataPath, $"{tableInfo.SheetName}{Extension}");
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }

            var dataList = await RecordParser.GetDataList(tableInfo, fileName);
            
            await RecordDataInserter.InsertAsync(tableInfo.RecordType, tableInfo.DbSetName, dataList, connection, transaction!);
        }

        await transaction!.CommitAsync();
    }
}