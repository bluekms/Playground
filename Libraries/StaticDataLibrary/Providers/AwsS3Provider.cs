using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using StaticDataLibrary.AwsS3Library;
using StaticDataLibrary.Options;

namespace StaticDataLibrary.Providers;

public class AwsS3Provider : IProviderBase
{
    public async Task RunAsync(StaticDataOptions options, string staticDataRoot, string tarFileName, string targetVersionPath)
    {
        await DownloadStaticData(options.AwsS3Provider!, staticDataRoot, tarFileName);
        
        var tarFileFullName = Path.Combine(staticDataRoot, tarFileName);
        UnpackStaticDataFile(tarFileFullName, targetVersionPath);
    }

    private static async Task DownloadStaticData(AwsS3ProviderOptions options, string staticDataRoot, string tarFileName)
    {
        var s3Services = new Aws3Services(
            options.RegionalDomainName,
            options.AwsAccessKeyId,
            options.AwsSecretAccessKey);
        var staticData = await s3Services.GetStaticDataAsync(options.BucketName, tarFileName);
        
        var tarFileFullName = Path.Combine(staticDataRoot, tarFileName);
        await File.WriteAllBytesAsync(tarFileFullName, staticData);
    }

    private static void UnpackStaticDataFile(string tarFileFullName, string targetVersionPath)
    {
        var stream = File.OpenRead(tarFileFullName);
        var gzStream = new GZipInputStream(stream);
        var tarArchive = TarArchive.CreateInputTarArchive(gzStream, Encoding.UTF8);
        
        tarArchive.ExtractContents(targetVersionPath);
        
        tarArchive.Close();
        gzStream.Close();
        stream.Close();
    }
}