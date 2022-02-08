using System.Threading.Tasks;
using CommonLibrary.Handlers;
using StackExchange.Redis;

namespace AccountServer.Handlers.Session
{
    public sealed record RemoveSessionIdCommand(string SessionId) : ICommand;

    public sealed class RemoveSessionIdHandler : ICommandHandler<RemoveSessionIdCommand>
    {
        private readonly IDatabase redis;

        public RemoveSessionIdHandler(IDatabase redis)
        {
            this.redis = redis;
        }

        public async Task ExecuteAsync(RemoveSessionIdCommand command)
        {
            var key = $"Session:{command.SessionId}";
            await redis.KeyDeleteAsync(key);
        }
    }
}