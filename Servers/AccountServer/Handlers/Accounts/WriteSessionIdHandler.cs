using System;
using System.Threading.Tasks;
using CommonLibrary.Handlers;
using StackExchange.Redis;

namespace AccountServer.Handlers.Accounts
{
    public sealed record WriteSessionIdCommand(
        string SessionId,
        string Authority) : ICommand;

    public class WriteSessionIdHandler : ICommandHandler<WriteSessionIdCommand>
    {
        private static TimeSpan _expire = new(0, 0, 5, 0);

        private readonly IDatabase _redis;

        public WriteSessionIdHandler(IDatabase redis)
        {
            _redis = redis;
        }

        public async Task ExecuteAsync(WriteSessionIdCommand command)
        {
            var key = $"Session:{command.SessionId}";
            await _redis.StringSetAsync(key, $"PlayerId:{command.Authority}");
            await _redis.KeyExpireAsync(key, _expire);
        }
    }
}