using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Worker;

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
        var taskCompletionSource = new TaskCompletionSource();
        await using (stoppingToken.Register(taskCompletionSource.SetResult))
        {
            await taskCompletionSource.Task;
        }

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

        LogTrace(logger, typeof(TWork), null);
        var work = (IWork)ActivatorUtilities.CreateInstance(provider, typeof(TWork));
        try
        {
            await work.RunAsync();
            LogInformation(logger, typeof(TWork), null);
        }
        catch (Exception e)
        {
            LogError(logger, typeof(TWork), e);
            throw;
        }
    }

    private static readonly Action<ILogger, Type, Exception?> LogTrace =
        LoggerMessage.Define<Type>(
            LogLevel.Trace,
            EventIdFactory.Create(ReservedLogEventId.WorkerStart),
            "{Work} is starting");

    private static readonly Action<ILogger, Type, Exception?> LogInformation =
        LoggerMessage.Define<Type>(
            LogLevel.Information,
            EventIdFactory.Create(ReservedLogEventId.WorkerFinished),
            "{Work} is finished");

    private static readonly Action<ILogger, Type, Exception?> LogError =
        LoggerMessage.Define<Type>(
            LogLevel.Error,
            EventIdFactory.Create(ReservedLogEventId.WorkServiceError),
            "{Work} is failed");
}