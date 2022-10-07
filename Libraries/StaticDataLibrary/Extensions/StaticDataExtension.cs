using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
        var recordInfoList = RecordFinder.Find<StaticDataContext>();
        foreach (var recordInfo in recordInfoList)
        {
            var fileName = Path.Combine(path, StaticDataPath, $"{recordInfo.SheetName}{Extension}");
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }

            var dataList = RecordParser.GetDataList(recordInfo, fileName);
            
            await RecordDataInserter.InsertAsync(recordInfo.DbSetName, recordInfo.RecordType, dataList, connection, transaction!);
        }

        await transaction!.CommitAsync();
    }
}