using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommonLibrary.Worker
{
    public static class WorkServiceExtension
    {
        public static IServiceCollection UseWorkService<TWork, TOptions>(this IServiceCollection services)
            where TWork : IWork
            where TOptions : class, IWorkServiceOptions<TWork>
        {
            services.AddSingleton<IWorkServiceOptions<TWork>, TOptions>();
            services.AddSingleton<IHostedService, WorkService<TWork>>();
            return services;
        }
    }
}
