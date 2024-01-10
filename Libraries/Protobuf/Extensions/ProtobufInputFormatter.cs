using Google.Protobuf;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Protobuf.Extensions;

public class ProtobufInputFormatter : InputFormatter
{
    public ProtobufInputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-protobuf"));
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
    {
        var httpContext = context.HttpContext;
        var request = httpContext.Request;

        if (request.ContentLength == 0)
        {
            return await InputFormatterResult.NoValueAsync();
        }

        try
        {
            using var streamReader = new StreamReader(request.Body);

            var messageType = context.ModelType;
            var message = Activator.CreateInstance(messageType) as IMessage;
            message.MergeFrom(streamReader.BaseStream);

            return await InputFormatterResult.SuccessAsync(message);
        }
        catch
        {
            return await InputFormatterResult.FailureAsync();
        }
    }
}
