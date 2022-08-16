using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.Extensions;

public static class ControllersExtension
{
    public static void UseControllers(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    }
}