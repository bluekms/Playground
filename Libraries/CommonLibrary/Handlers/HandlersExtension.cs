using CommonLibrary.Handlers.Decorators;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.Handlers
{
    public static class HandlersExtension
    {
        public static IServiceCollection UseHandlers(
            this IServiceCollection services,
            GenericDerivedTypeSelector types)
        {
            foreach (var (type, serviceType) in types.GetGenericInheritedTypes(typeof(IQueryHandler<,>)))
            {
                services.AddTransient(serviceType, type);
            }
            
            foreach (var (type, serviceType) in types.GetGenericInheritedTypes(typeof(ICommandHandler<>)))
            {
                services.AddTransient(serviceType, type);
            }
            
            foreach (var (type, serviceType) in types.GetGenericInheritedTypes(typeof(ICommandHandler<,>)))
            {
                services.AddTransient(serviceType, type);
            }
            
            foreach (var (type, serviceType) in types.GetGenericInheritedTypes(typeof(IRuleChecker<>)))
            {
                services.AddTransient(serviceType, type);
            }

            return services;
        }
    }
}