using Microsoft.Extensions.DependencyInjection;

namespace Protobuf.Extensions;

public static class ProtobufExtension
{
    public static void UseProtobuf(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddControllers(options =>
        {
            options.InputFormatters.Add(new ProtobufInputFormatter());
            options.OutputFormatters.Add(new ProtobufOutputFormatter());
        });
    }
}
