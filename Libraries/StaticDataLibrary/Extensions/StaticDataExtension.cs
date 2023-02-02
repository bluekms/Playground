using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StaticDataLibrary.AwsS3Library;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.Extensions;

public static class StaticDataExtension
{
    public static async void UseStaticData(this IServiceCollection services, IConfigurationSection staticDataSection)
    {
        var options = staticDataSection.Get<StaticDataOptions>();
        
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var staticDataPath = Path.Combine(folder, "StaticData");
        var di = new DirectoryInfo(staticDataPath);
        if (!di.Exists)
        {
            di.Create();
        }
        
        var fileName = $"{options.DataName}-{options.Version}.tar.gz";
        var tarFileName = Path.Combine(staticDataPath, fileName);
        var unpackPath = Path.Combine(staticDataPath, options.Version);

        if (options.ForceUpdate)
        {
            RemoveOldFiles(tarFileName, unpackPath);
            await DownloadStaticData(options.S3Provider, staticDataPath, fileName);
            UnpackStaticDataFile(tarFileName, unpackPath);    
        }
        
        await InitializeSqlite(services, unpackPath);
    }

    private static void RemoveOldFiles(string tarFile, string staticDataPath)
    {
        File.Delete(tarFile);

        var di = new DirectoryInfo(staticDataPath);
        if (!di.Exists)
        {
            return;
        }
        
        foreach (var file in di.GetFiles())
        {
            file.Delete();
        }
        
        di.Delete();
    }

    private static async Task DownloadStaticData(StaticDataOptions.AwsS3Provider options, string path, string fileName)
    {
        // TODO goto appsettings
        var s3Services = new Aws3Services(
            options.RegionalDomainName,
            options.AwsAccessKeyId,
            options.AwsSecretAccessKey);
        var staticData = await s3Services.GetStaticDataAsync("nk-r-and-d", fileName);
        
        var localFileName = Path.Combine(path, fileName);
        await File.WriteAllBytesAsync(localFileName, staticData);
    }
    
    private static void UnpackStaticDataFile(string tarFile, string staticDataPath)
    {
        var stream = File.OpenRead(tarFile);
        var gzStream = new GZipInputStream(stream);
        var tarArchive = TarArchive.CreateInputTarArchive(gzStream, Encoding.UTF8);
        
        tarArchive.ExtractContents(staticDataPath);
        
        tarArchive.Close();
        gzStream.Close();
        stream.Close();
    }

    private static async Task InitializeSqlite(IServiceCollection services, string staticDataPath)
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
}