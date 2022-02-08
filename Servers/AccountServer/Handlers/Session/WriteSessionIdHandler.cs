using System;
using System.Threading.Tasks;
using CommonLibrary.Handlers;
using StackExchange.Redis;

namespace AccountServer.Handlers.Session
{
    public sealed record WriteSessionIdCommand(
        string SessionId,
        string Authority) : ICommand;

    public sealed class WriteSessionIdHandler : ICommandHandler<WriteSessionIdCommand>
    {
        private static TimeSpan DefaultExpire = new(0, 0, 5, 0);

        private readonly IDatabase redis;

        public WriteSessionIdHandler(IDatabase redis)
        {
            this.redis = redis;
        }

        public async Task ExecuteAsync(WriteSessionIdCommand command)
        {
            var key = $"Session:{command.SessionId}";
            await redis.StringSetAsync(key, $"PlayerId:{command.Authority}");
            await redis.KeyExpireAsync(key, DefaultExpire);
        }
    }
}