namespace StaticDataLibrary.Options;

public class StaticDataOptions
{
    public const string ConfigurationSection = "StaticData";

    public string DataName { get; set; } = null!;
    public string Version { get; set; } = null!;
    public bool ForceUpdate { get; set; }
    public ProviderTypes ProviderType { get; set; }
    public AwsS3ProviderOptions? AwsS3Provider { get; set; }
    public CsvFilesProviderOptions? CsvFilesProvider { get; set; }
    public TarFileProviderOptions? TarFileProvider { get; set; }
    
    public enum ProviderTypes
    {
        AwsS3 = 0,
        CsvFiles = 1,
        TarFile = 2,
    }
}