namespace StaticDataLibrary;

public class StaticDataOptions
{
    public const string SectionName = "StaticData";
    
    public string DataName { get; set; } = null!;
    public string Version { get; set; } = null!;
    public bool ForceUpdate { get; set; }
    public Providers Provider { get; set; }
    public AwsS3Provider? S3Provider { get; set; } = null;
    
    public enum Providers
    {
        LocalFile,
        AwsS3,
    }

    public class LocalFileProvider
    {
        public string FilePath { get; } = null!;
    }

    public class AwsS3Provider
    {
        public string RegionalDomainName { get; set; } = null!;
        public string AwsAccessKeyId { get; set; } = null!;
        public string AwsSecretAccessKey { get; set; } = null!;
    }
}