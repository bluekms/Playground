using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using StaticDataLibrary.Options;

namespace StaticDataLibrary.Providers;

public class TarFileProvider : IProviderBase
{
    public Task RunAsync(StaticDataOptions options, string staticDataRoot, string tarFileName, string targetVersionPath)
    {
        var fileFullName = Path.Combine(options.TarFileProvider!.TarFilePath, tarFileName);
        var fileInfo = new FileInfo(fileFullName);
        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException(fileFullName);
        }
        
        var tarFileFullName = Path.Combine(staticDataRoot, tarFileName);
        fileInfo.CopyTo(tarFileFullName);
        
        UnpackStaticDataFile(tarFileFullName, targetVersionPath);
        
        return Task.CompletedTask;
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