using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StaticDataLibrary.AwsS3Library;
using StaticDataLibrary.Options;
using StaticDataLibrary.Providers;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.Extensions;

public static class StaticDataExtension
{
    public static async Task UseStaticDataAsync(this IServiceCollection services, IConfigurationSection staticDataSection)
    {
        var options = staticDataSection.Get<StaticDataOptions>()!;
        
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var staticDataRoot = Path.Combine(appDataPath, options.DataName);
        var rootDi = new DirectoryInfo(staticDataRoot);
        if (!rootDi.Exists)
        {
            rootDi.Create();
        }
        
        var tarFileName = $"{options.DataName}-{options.Version}.tar.gz";
        var targetVersionPath = Path.Combine(staticDataRoot, options.Version);
        var targetVersionDi = new DirectoryInfo(targetVersionPath);
        if (targetVersionDi.Exists && targetVersionDi.GetFiles().Length > 0)
        {
            if (options.ForceUpdate)
            {
                await Clean(options, staticDataRoot, tarFileName, targetVersionPath);
            }
        }
        else
        {
            await Clean(options, staticDataRoot, tarFileName, targetVersionPath);
        }
        
        await InitializeSqlite(services, targetVersionPath);
    }

    private static async Task Clean(StaticDataOptions options, string staticDataRoot, string tarFileName, string targetVersionPath)
    {
        RemoveOldFiles(tarFileName, targetVersionPath);

        var provider = options.ProviderType switch
        {
            StaticDataOptions.ProviderTypes.AwsS3 => (IProviderBase)new AwsS3Provider(),
            StaticDataOptions.ProviderTypes.CsvFiles => new CsvFilesProvider(),
            StaticDataOptions.ProviderTypes.TarFile => new TarFileProvider(),
            _ => null,
        };

        await provider!.RunAsync(options, staticDataRoot, tarFileName, targetVersionPath);
    }

    private static void RemoveOldFiles(string tarFileName, string tarVersionPath)
    {
        File.Delete(tarFileName);

        var di = new DirectoryInfo(tarVersionPath);
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