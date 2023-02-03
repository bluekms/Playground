using StaticDataLibrary.Options;

namespace StaticDataLibrary.Providers;

public class CsvFilesProvider : IProviderBase
{
    public Task RunAsync(StaticDataOptions options, string staticDataRoot, string tarFileName, string targetVersionPath)
    {
        var srcDi = new DirectoryInfo(options.CsvFilesProvider!.Path);
        if (!srcDi.Exists || srcDi.GetFiles().Length == 0)
        {
            throw new FileNotFoundException($"{options.CsvFilesProvider.Path}");
        }

        var dstPath = Path.Combine(staticDataRoot, options.Version);
        var dstDi = new DirectoryInfo(dstPath);
        if (!dstDi.Exists)
        {
            dstDi.Create();
        }

        foreach (var file in srcDi.GetFiles("*.csv"))
        {
            var dst = Path.Combine(dstPath, file.Name);
            file.CopyTo(dst);
        }

        return Task.CompletedTask;
    }
}