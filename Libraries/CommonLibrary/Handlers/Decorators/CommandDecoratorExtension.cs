using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.Handlers.Decorators;

public static class CommandDecoratorExtension
{
    public static IServiceCollection UseCommandDecorator(this IServiceCollection services)
    {
        services.Decorate(typeof(ICommandHandler<>), typeof(CommandDecorator<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(CommandDecorator<,>));
        return services;
    }
}