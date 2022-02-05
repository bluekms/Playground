using System.Linq;
using System.Reflection;
using AccountServer.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace AccountServer.ServiceExtensions
{
    public static class HandlersExtension
    {
        public static IServiceCollection AddHandlers(this IServiceCollection serviceCollection, Assembly assembly)
        {
            var genericTypes = assembly.GetTypes()
                .Where(x => x.IsPublic)
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => x.GetInterfaces()
                    .Where(y => y.IsGenericType)
                    .Any(y => typeof(IHandlerBase).IsAssignableFrom(y)))
                .ToList();

            foreach (var t in genericTypes)
            {
                var serviceType = t.GetInterfaces().First(x => typeof(IHandlerBase).IsAssignableFrom(x));

                serviceCollection.AddTransient(serviceType, t);
            }

            return serviceCollection;
        }
    }
}