using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StaticDataLibrary.AwsS3Library;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.Extensions;

public static class StaticDataExtension
{
    public static async void UseStaticData(this IServiceCollection services, string name, string version = "latest")
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var staticDataPath = Path.Combine(folder, "StaticData");
        var fileName = $"{name}-{version}.tar.gz";
        await DownloadStaticData(staticDataPath, fileName);

        var targzFileName = Path.Combine(staticDataPath, fileName);
        var unpackPath = Path.Combine(staticDataPath, version);
        UnpackStaticDataFile(targzFileName, unpackPath);
        
        await InitializeSqlite(services, unpackPath);
    }

    private static async Task DownloadStaticData(string path, string fileName)
    {
        // TODO goto appsettings
        var s3Services = new Aws3Services(
            "",
            "",
            "");
        var staticData = await s3Services.GetStaticDataAsync("", fileName);
        
        var localFileName = Path.Combine(path, fileName);
        await File.WriteAllBytesAsync(localFileName, staticData);
    }

    private static async Task InitializeSqlite(this IServiceCollection services, string staticDataPath)
    {
        services.AddEntityFrameworkSqlite().AddDbContext<StaticDataContext>();
        
        await using var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<StaticDataContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        
        await using SqliteConnection connection = new SqliteConnection(context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync() as SqliteTransaction;
        
        var tableInfoList = TableFinder.Find<StaticDataContext>();
        foreach (var tableInfo in tableInfoList)
        {
            var fileName = Path.Combine(staticDataPath, $"{tableInfo.SheetName}.csv");
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }

            var dataList = await RecordParser.GetDataListAsync(tableInfo, fileName);
            
            await RecordSqlExecutor.InsertAsync(connection, tableInfo, dataList, transaction!);
        }

        await transaction!.CommitAsync();
    }
    
    private static void UnpackStaticDataFile(string targetFile, string staticDataPath)
    {
        var stream = File.OpenRead(targetFile);
        var gzStream = new GZipInputStream(stream);
        var tarArchive = TarArchive.CreateInputTarArchive(gzStream);
        
        tarArchive.ExtractContents(staticDataPath);
        
        tarArchive.Close();
        gzStream.Close();
        stream.Close();
    }
}