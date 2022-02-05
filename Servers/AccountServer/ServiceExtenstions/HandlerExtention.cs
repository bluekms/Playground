using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AccountServer.ServiceExtenstions
{
    public static class HandlerExtention
    {
        public static IServiceCollection AddTransientHandler(this IServiceCollection serviceCollection, Type handlerType)
        {
            var genericTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.IsPublic)
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => x.GetInterfaces()
                    .Where(y => y.IsGenericType)
                    .Any(y => y.GetGenericTypeDefinition() == handlerType))
                .ToList();

            foreach (var t in genericTypes)
            {
                var serviceType = t.GetInterfaces().First(x => x.GetGenericTypeDefinition() == handlerType);
                serviceCollection.AddTransient(serviceType, t);
            }

            return serviceCollection;
        }
    }
}