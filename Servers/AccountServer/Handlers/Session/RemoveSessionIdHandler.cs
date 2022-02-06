using System.Threading.Tasks;
using CommonLibrary.Handlers;
using StackExchange.Redis;

namespace AccountServer.Handlers.Session
{
    public sealed record RemoveSessionIdCommand(string SessionId) : ICommand;

    public class RemoveSessionIdHandler : ICommandHandler<RemoveSessionIdCommand>
    {
        private readonly IDatabase _redis;

        public RemoveSessionIdHandler(IDatabase redis)
        {
            _redis = redis;
        }

        public async Task ExecuteAsync(RemoveSessionIdCommand command)
        {
            var key = $"Session:{command.SessionId}";
            await _redis.KeyDeleteAsync(key);
        }
    }
}