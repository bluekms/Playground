using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.Extensions.Protobuf;

public static class ProtobufExtension
{
    public static void UseProtobuf(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.InputFormatters.Add(new ProtobufInputFormatter());
            options.OutputFormatters.Add(new ProtobufOutputFormatter());
        });
    }
}