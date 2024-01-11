namespace StaticDataLibrary.Options;

public class AwsS3ProviderOptions
{
    public string RegionalDomainName { get; set; } = null!;
    public string AwsAccessKeyId { get; set; } = null!;
    public string AwsSecretAccessKey { get; set; } = null!;
    public string BucketName { get; set; } = null!;
}
