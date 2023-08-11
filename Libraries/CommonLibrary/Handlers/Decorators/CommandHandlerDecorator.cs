using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Handlers.Decorators;

public sealed class CommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> handler;
    private readonly ILogger<CommandHandlerDecorator<TCommand>> logger;
    private readonly ActivitySource activitySource;

    public CommandHandlerDecorator(
        ICommandHandler<TCommand> handler,
        ILogger<CommandHandlerDecorator<TCommand>> logger,
        ActivitySource activitySource)
    {
        this.handler = handler;
        this.logger = logger;
        this.activitySource = activitySource;
    }

    // TODO cancellationToken 사용하지 않도록 (데드락 검토 필요)
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
                LogInformation(logger, typeof(TCommand), command, null);
            }
            else
            {
                LogWarning(logger, typeof(TCommand), command, null);
            }
        }
        catch (Exception e)
        {
            LogError(logger, typeof(TCommand), command, e);
            throw;
        }
    }

    private static readonly TimeSpan Threshold = TimeSpan.FromMilliseconds(200);

    private static readonly Action<ILogger, Type, TCommand, Exception?> LogInformation =
        LoggerMessage.Define<Type, TCommand>(
            LogLevel.Information,
            EventIdFactory.Create(ReservedLogEventId.CommandHandlerInformation),
            "Command {CommandType} {@Command}");

    private static readonly Action<ILogger, Type, TCommand, Exception?> LogWarning =
        LoggerMessage.Define<Type, TCommand>(
            LogLevel.Warning,
            EventIdFactory.Create(ReservedLogEventId.CommandHandlerWarning),
            "Command {CommandType} {@Command}");

    private static readonly Action<ILogger, Type, TCommand, Exception?> LogError =
        LoggerMessage.Define<Type, TCommand>(
            LogLevel.Error,
            EventIdFactory.Create(ReservedLogEventId.CommandHandlerError),
            "Command failed {CommandType} {@Command}");
}