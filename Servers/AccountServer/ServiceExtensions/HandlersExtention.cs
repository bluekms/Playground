using System.Linq;
using System.Reflection;
using AccountServer.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace AccountServer.ServiceExtensions
{
    public static class HandlersExtension
    {
        public static IServiceCollection AddHandlers(this IServiceCollection serviceCollection)
        {
            var genericTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.IsPublic)
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => x.GetInterfaces()
                    .Where(y => y.IsGenericType)
                    .Any(y => (
                        y.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) ||
                        y.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                        y.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                        y.GetGenericTypeDefinition() == typeof(IRuleChecker<>))))
                .ToList();

            foreach (var t in genericTypes)
            {
                var serviceType = t.GetInterfaces().First(x => (
                    x.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) ||
                    x.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                    x.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                    x.GetGenericTypeDefinition() == typeof(IRuleChecker<>)));

                serviceCollection.AddTransient(serviceType, t);
            }

            return serviceCollection;
        }
    }
}