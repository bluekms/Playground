using System.Net;
using Amazon.S3;
using Amazon.S3.Model;

namespace StaticDataLibrary.AwsS3Library;

// https://www.c-sharpcorner.com/article/uploaddownloaddelete-files-from-aws-s3-using-net-core-api/

public class Aws3Services
{
    private readonly IAmazonS3 awsS3Client;

    public Aws3Services(string regionalDomainName, string awsAccessKeyId, string awsSecretAccessKey)
    {
        var config = new AmazonS3Config()
        {
            ServiceURL = regionalDomainName,
            MaxConnectionsPerServer = 32,
        };
        
        awsS3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, config);
    }

    public async Task<byte[]> GetStaticDataAsync(string bucketName, string fileName)
    {
        var request = new GetObjectRequest()
        {
            BucketName = bucketName,
            Key = fileName,
        };

        using var response = await awsS3Client.GetObjectAsync(request);
        if (response.HttpStatusCode is < HttpStatusCode.OK or >= HttpStatusCode.Ambiguous)
        {
            throw new HttpRequestException($"{fileName} is not found.");
        }

        await using var ms = new MemoryStream();
        await response.ResponseStream.CopyToAsync(ms);
        return ms.ToArray();
    }
}