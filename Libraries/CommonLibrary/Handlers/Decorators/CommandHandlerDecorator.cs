using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

namespace CommonLibrary.Handlers.Decorators;

public sealed class CommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private static readonly TimeSpan Threshold = TimeSpan.FromMilliseconds(100);

    private readonly ActivitySource activitySource;
    private readonly ICommandHandler<TCommand> handler;
    private readonly ILogger<CommandHandlerDecorator<TCommand>> logger;
    private readonly IHostEnvironment hostEnvironment;

    public CommandHandlerDecorator(
        ActivitySource activitySource,
        ICommandHandler<TCommand> handler,
        ILogger<CommandHandlerDecorator<TCommand>> logger,
        IHostEnvironment hostEnvironment)
    {
        this.activitySource = activitySource;
        this.handler = handler;
        this.logger = logger;
        this.hostEnvironment = hostEnvironment;
    }

    public async Task ExecuteAsync(TCommand command)
    {
        using var activity = activitySource.StartActivity($"Command {typeof(TCommand).Name}");
        activity?.AddTag("Playground.Command.Type", typeof(TCommand).Name);

        var sw = Stopwatch.StartNew();
        try
        {
            await handler.ExecuteAsync(command);
            sw.Stop();

            if (sw.Elapsed < Threshold)
            {
                LogInformation(logger, command, sw.Elapsed.TotalMilliseconds, null);
            }
            else
            {
                if (hostEnvironment.IsProduction())
                {
                    LogWarning(logger, typeof(TCommand), sw.Elapsed.TotalMilliseconds, null);
                }
                else
                {
                    LogWarningForDev(logger, command, sw.Elapsed.TotalMilliseconds, null);
                }
            }
        }
        catch (Exception e)
        {
            if (hostEnvironment.IsProduction())
            {
                LogError(logger, typeof(TCommand), sw.Elapsed.TotalMilliseconds, e);
            }
            else
            {
                LogErrorForDev(logger, command, sw.Elapsed.TotalMilliseconds, e);
            }

            throw;
        }
    }

    private static readonly Action<ILogger, TCommand, double, Exception?> LogInformation =
        LoggerMessage.Define<TCommand, double>(
            LogLevel.Information,
            EventIdFactory.Create(ReservedLogEventId.CommandHandlerInformation),
            "Command: {@Command}, Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, Type, double, Exception?> LogWarning =
        LoggerMessage.Define<Type, double>(
            LogLevel.Warning,
            EventIdFactory.Create(ReservedLogEventId.CommandHandlerWarning),
            "Command Too Slow. {@CommandType} Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, TCommand, double, Exception?> LogWarningForDev =
        LoggerMessage.Define<TCommand, double>(
            LogLevel.Warning,
            EventIdFactory.Create(ReservedLogEventId.CommandHandlerWarning),
            "Command Too Slow. Command: {@Command}, Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, Type, double, Exception?> LogError =
        LoggerMessage.Define<Type, double>(
            LogLevel.Error,
            EventIdFactory.Create(ReservedLogEventId.CommandHandlerError),
            "Command Failed! {@CommandType} Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, TCommand, double, Exception?> LogErrorForDev =
        LoggerMessage.Define<TCommand, double>(
            LogLevel.Error,
            EventIdFactory.Create(ReservedLogEventId.CommandHandlerError),
            "Command Failed! Command: {@Command}, Elapsed: {Elapsed:0.0000}ms");
}
