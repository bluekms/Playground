using System.Reflection;
using CommonLibrary;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace AuthServer.Extensions
{
    public static class MapsterExtension
    {
        public static void UseMapster(this IServiceCollection services, Assembly assembly)
        {
            services.AddMapster(config =>
            {
                config.RequireDestinationMemberSource = true;
                config.Default.MapToConstructor(true);
            });

            services.AddMapsterRegisters(Assembly.GetExecutingAssembly());
        }
    }
}