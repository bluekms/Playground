using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Worker
{
    public class WorkService<TWork> : BackgroundService
        where TWork : IWork
    {
        private readonly ILogger<WorkService<TWork>> logger;
        private readonly IWorkServiceOptions<TWork> options;
        private readonly IServiceProvider provider;

        public WorkService(
            ILogger<WorkService<TWork>> logger,
            IWorkServiceOptions<TWork> options,
            IServiceProvider provider)
        {
            this.logger = logger;
            this.options = options;
            this.provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(options.Delay, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                using var innerScope = provider.CreateScope();
                await RunAsync(innerScope.ServiceProvider);

                await Task.Delay(options.Interval, stoppingToken);
            }
        }

        private async Task RunAsync(IServiceProvider provider)
        {
            var activitySource = provider.GetRequiredService<ActivitySource>();
            using var activity = activitySource.StartActivity(typeof(TWork).Name);

            logger.LogTrace("{Work} is starting", typeof(TWork));
            var work = (IWork)ActivatorUtilities.CreateInstance(provider, typeof(TWork));
            try
            {
                await work.RunAsync();
                logger.LogInformation("{Work} is finished.", typeof(TWork));
            }
            catch (Exception e)
            {
                logger.LogError(e, "{Work} is failed", typeof(TWork));
                throw;
            }
        }
    }
}