using System.Reflection;
using CommonLibrary.Handlers.Decorators;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.Handlers;

public static class HandlersExtension
{
    public static IServiceCollection UseHandlers(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var typeSelector = new GenericDerivedTypeSelector(assemblies);

        services.Decorate(typeof(IQueryHandler<,>), typeof(QueryHandlerDecorator<,>));
        foreach (var (type, serviceType) in typeSelector.GetGenericInheritedTypes(typeof(IQueryHandler<,>)))
        {
            services.AddTransient(serviceType, type);
        }

        services.Decorate(typeof(ICommandHandler<>), typeof(CommandHandlerDecorator<>));
        foreach (var (type, serviceType) in typeSelector.GetGenericInheritedTypes(typeof(ICommandHandler<>)))
        {
            services.AddTransient(serviceType, type);
        }

        services.Decorate(typeof(ICommandHandler<,>), typeof(CommandHandlerDecorator<,>));
        foreach (var (type, serviceType) in typeSelector.GetGenericInheritedTypes(typeof(ICommandHandler<,>)))
        {
            services.AddTransient(serviceType, type);
        }

        services.Decorate(typeof(IRuleChecker<>), typeof(RuleCheckerDecorator<>));
        foreach (var (type, serviceType) in typeSelector.GetGenericInheritedTypes(typeof(IRuleChecker<>)))
        {
            services.AddTransient(serviceType, type);
        }

        services.Decorate(typeof(IRuleChecker<,>), typeof(RuleCheckerDecorator<,>));
        foreach (var (type, serviceType) in typeSelector.GetGenericInheritedTypes(typeof(IRuleChecker<,>)))
        {
            services.AddTransient(serviceType, type);
        }

        return services;
    }
}
