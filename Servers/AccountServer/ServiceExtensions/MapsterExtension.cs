using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AccountServer.ServiceExtensions
{
    public static class MapsterExtension
    {
        public static IServiceCollection AddMapster(this IServiceCollection serviceCollection, Action<TypeAdapterConfig> configure)
        {
            serviceCollection.AddSingleton(provider =>
            {
                var typeAdapterConfig = new TypeAdapterConfig();
                configure(typeAdapterConfig);

                var registers = provider.GetRequiredService<IEnumerable<IRegister>>();
                typeAdapterConfig.Apply(registers);

                typeAdapterConfig.Compile();
                return typeAdapterConfig;
            });

            serviceCollection.AddScoped<IMapper, ServiceMapper>();
            return serviceCollection;
        }

        public static IServiceCollection AddMapsterRegisters(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(x => typeof(IRegister).IsAssignableFrom(x))
                .Where(x => x.IsClass)
                .Where(x => !x.IsAbstract);

            foreach (var t in types)
            {
                services.AddTransient(typeof(IRegister), t);
            }

            return services;
        }
    }
}