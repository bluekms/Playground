using System.Threading.Tasks;
using CommonLibrary.Handlers;
using StackExchange.Redis;

namespace AuthServer.Handlers.Session
{
    public sealed record DeleteSessionIdCommand(string SessionId) : ICommand;

    public sealed class DeleteSessionIdHandler : ICommandHandler<DeleteSessionIdCommand>
    {
        private readonly IDatabase redis;

        public DeleteSessionIdHandler(IDatabase redis)
        {
            this.redis = redis;
        }

        public async Task ExecuteAsync(DeleteSessionIdCommand command)
        {
            var key = $"Session:{command.SessionId}";
            await redis.KeyDeleteAsync(key);
        }
    }
}