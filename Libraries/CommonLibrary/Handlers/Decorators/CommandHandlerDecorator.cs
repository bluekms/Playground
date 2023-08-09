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

    public async Task ExecuteAsync(TCommand command)
    {
        try
        {
            using var activity = activitySource.StartActivity($"Command {typeof(TCommand).Name}");
            activity?.AddTag("Playground.Command.Type", typeof(TCommand).Name);

            await handler.ExecuteAsync(command);
            logger.LogInformation("Command {CommandType} {@Command}", typeof(TCommand).Name, command);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Command failed {CommandType} {@Command}", typeof(TCommand).Name, command);
            throw;
        }
    }
}

public sealed class CommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand
{
    private static readonly Action<ILogger, Type, object, Exception?> logInformation =
        LoggerMessage.Define<Type, object>(
            LogLevel.Information,
            new EventId(1, "Command"),
            "Command {CommandType} {@Command}");

    private readonly ICommandHandler<TCommand, TResult> target;
    private readonly ILogger<CommandHandlerDecorator<TCommand, TResult>> logger;
    private readonly ActivitySource activitySource;

    public CommandHandlerDecorator(
        ICommandHandler<TCommand, TResult> target,
        ILogger<CommandHandlerDecorator<TCommand, TResult>> logger,
        ActivitySource activitySource)
    {
        this.target = target;
        this.logger = logger;
        this.activitySource = activitySource;
    }

    public async Task<TResult> ExecuteAsync(TCommand command)
    {
        try
        {
            using var activity = activitySource.StartActivity($"Command {typeof(TCommand).Name}");
            activity?.AddTag("Playground.Command.Type", typeof(TCommand).Name);

            var result = await target.ExecuteAsync(command);

            logInformation(logger, typeof(TCommand), command, null);

            //logger.LogInformation("Command {CommandType} {@Command}", typeof(TCommand).Name, command);
            logger.LogTrace("Command Result {@CommandResult}", result);

            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Command failed {CommandType} {@Command}", typeof(TCommand).Name, command);
            throw;
        }
    }
}