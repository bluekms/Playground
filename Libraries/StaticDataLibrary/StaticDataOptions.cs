namespace StaticDataLibrary;

public class StaticDataOptions
{
    public const string SectionName = "StaticData";

    public string DataName { get; set; } = null!;
    public string Version { get; set; } = null!;
    public bool ForceUpdate { get; set; }
    public ProviderTypes ProviderType { get; set; }
    public AwsS3Provider? S3Provider { get; set; }
    public CsvFilesProvider? CsvProvider { get; set; }
    
    public enum ProviderTypes
    {
        AwsS3,
        CsvFiles,
    }

    public class CsvFilesProvider
    {
        public string Path { get; set; } = null!;
    }

    public class AwsS3Provider
    {
        public string RegionalDomainName { get; set; } = null!;
        public string AwsAccessKeyId { get; set; } = null!;
        public string AwsSecretAccessKey { get; set; } = null!;
        public string BucketName { get; set; } = null!;
    }
}