using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

namespace CommonLibrary.Handlers.Decorators;

public sealed class CommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand
{
    private static readonly TimeSpan Threshold = TimeSpan.FromMilliseconds(100);

    private readonly ActivitySource activitySource;
    private readonly ICommandHandler<TCommand, TResult> handler;
    private readonly ILogger<CommandHandlerDecorator<TCommand, TResult>> logger;
    private readonly IHostEnvironment hostEnvironment;

    public CommandHandlerDecorator(
        ActivitySource activitySource,
        ICommandHandler<TCommand, TResult> handler,
        ILogger<CommandHandlerDecorator<TCommand, TResult>> logger,
        IHostEnvironment hostEnvironment)
    {
        this.activitySource = activitySource;
        this.handler = handler;
        this.logger = logger;
        this.hostEnvironment = hostEnvironment;
    }

    public async Task<TResult> ExecuteAsync(TCommand command)
    {
        using var activity = activitySource.StartActivity($"Command {typeof(TCommand).Name}");
        activity?.AddTag("Playground.Command.Type", typeof(TCommand).Name);

        var sw = Stopwatch.StartNew();
        try
        {
            var result = await handler.ExecuteAsync(command);
            sw.Stop();

            if (sw.Elapsed < Threshold)
            {
                LogInformation(logger, command, result, sw.Elapsed.TotalMilliseconds, null);
            }
            else
            {
                if (hostEnvironment.IsProduction())
                {
                    LogWarning(logger, typeof(TCommand), sw.Elapsed.TotalMilliseconds, null);
                }
                else
                {
                    LogWarningForDev(logger, command, result, sw.Elapsed.TotalMilliseconds, null);
                }
            }

            return result;
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

    private static readonly Action<ILogger, TCommand, TResult, double, Exception?> LogInformation =
        LoggerMessage.Define<TCommand, TResult, double>(
            LogLevel.Information,
            EventIdFactory.Create(ReservedLogEventId.CommandHandlerInformation),
            "Command: {@Command}, Result: {@Result}, Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, Type, double, Exception?> LogWarning =
        LoggerMessage.Define<Type, double>(
            LogLevel.Warning,
            EventIdFactory.Create(ReservedLogEventId.CommandHandlerWarning),
            "Command Too Slow. {@CommandType} Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, TCommand, TResult, double, Exception?> LogWarningForDev =
        LoggerMessage.Define<TCommand, TResult, double>(
            LogLevel.Warning,
            EventIdFactory.Create(ReservedLogEventId.CommandHandlerWarning),
            "Command Too Slow. Command: {@Command}, Result: {@Result}, Elapsed: {Elapsed:0.0000}ms");

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
