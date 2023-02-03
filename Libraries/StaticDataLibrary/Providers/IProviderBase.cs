using StaticDataLibrary.Options;

namespace StaticDataLibrary.Providers;

public interface IProviderBase
{
    Task RunAsync(StaticDataOptions options, string staticDataRoot, string tarFileName, string targetVersionPath);
}