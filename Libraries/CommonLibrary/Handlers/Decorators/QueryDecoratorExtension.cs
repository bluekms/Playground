using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.Handlers.Decorators;

public static class QueryDecoratorExtension
{
    public static IServiceCollection UseQueryDecorator(this IServiceCollection services)
    {
        services.Decorate(typeof(IQueryHandler<,>), typeof(QueryHandlerDecorator<,>));
        return services;
    }
}