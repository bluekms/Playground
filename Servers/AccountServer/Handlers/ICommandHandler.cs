using System.Threading.Tasks;

namespace AccountServer.Handlers
{
    public interface ICommand
    {
    }

    public interface ICommandHandler<in TCommand> : IHandlerBase
        where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand command);
    }

    public interface ICommandHandler<in TCommand, TResult> : IHandlerBase
        where TCommand : ICommand
    {
        Task<TResult> ExecuteAsync(TCommand command);
    }
}