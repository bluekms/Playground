namespace StaticDataLibrary.Providers;

public class CsvFileProvider : IProviderBase
{
    public Task RunAsync(StaticDataOptions options, string staticDataRoot, string tarFileName, string targetVersionPath)
    {
        var srcDi = new DirectoryInfo(options.CsvProvider!.Path);
        if (!srcDi.Exists || srcDi.GetFiles().Length == 0)
        {
            throw new FileNotFoundException($"{options.CsvProvider.Path}");
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
            File.Copy(file.FullName, dst);
        }

        return Task.CompletedTask;
    }
}