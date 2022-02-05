using System.Threading.Tasks;

namespace AccountServer.Handlers
{
    public interface ICommand
    {
    }

    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand command);
    }

    public interface ICommandHandler<in TCommand, TResult>
        where TCommand : ICommand
    {
        Task<TResult> ExecuteAsync(TCommand command);
    }
}