using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Handlers.Decorators;

internal sealed class CommandDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> handler;
    private readonly ILogger<CommandDecorator<TCommand>> logger;
    private readonly ActivitySource activitySource;

    public CommandDecorator(
        ICommandHandler<TCommand> handler,
        ILogger<CommandDecorator<TCommand>> logger,
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

internal sealed class CommandDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand, TResult> target;
    private readonly ILogger<CommandDecorator<TCommand, TResult>> logger;
    private readonly ActivitySource activitySource;

    public CommandDecorator(
        ICommandHandler<TCommand, TResult> target,
        ILogger<CommandDecorator<TCommand, TResult>> logger,
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
            logger.LogInformation("Command {CommandType} {@Command}", typeof(TCommand).Name, command);
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