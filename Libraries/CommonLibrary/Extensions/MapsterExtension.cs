using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.Extensions;

public static class MapsterExtension
{
    public static void UseMapster(this IServiceCollection services)
    {
        services.AddMapster(config =>
        {
            config.RequireDestinationMemberSource = true;
            config.Default.MapToConstructor(true);
        });

        services.AddMapsterRegisters(Assembly.GetExecutingAssembly());
    }
}
