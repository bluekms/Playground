using Google.Protobuf;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace CommonLibrary.Extensions.Protobuf;

public class ProtobufOutputFormatter : OutputFormatter
{
    public ProtobufOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-protobuf"));
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
    {
        if (context.Object is not IMessage message)
        {
            return;
        }

        using var outputStream = new MemoryStream();
        message.WriteTo(outputStream);

        var httpContext = context.HttpContext;
        var response = httpContext.Response;
        response.ContentLength = outputStream.Length;
        response.ContentType = "application/x-protobuf";
        response.Headers.Add("Content-Disposition", $"attachment; filename={Guid.NewGuid()}.pb");

        outputStream.Position = 0;
        await outputStream.CopyToAsync(response.Body);
    }

    protected override bool CanWriteType(Type? type)
    {
        return typeof(IMessage).IsAssignableFrom(type);
    }
}