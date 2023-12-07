using Microsoft.Extensions.Hosting;
using Serilog;

namespace CommonLibrary;

public static class SerilogExtension
{
    public static IHostBuilder UseLogger(this IHostBuilder builder)
    {
        builder.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        return builder;
    }
}
