using System;
using System.Threading.Tasks;
using CommonLibrary.Handlers;
using StackExchange.Redis;

namespace AccountServer.Handlers.Session
{
    public sealed record InsertSessionIdCommand(
        string SessionId,
        string UserRole) : ICommand;

    public sealed class InsertSessionIdHandler : ICommandHandler<InsertSessionIdCommand>
    {
        private static TimeSpan DefaultExpire = new(0, 0, 5, 0);

        private readonly IDatabase redis;

        public InsertSessionIdHandler(IDatabase redis)
        {
            this.redis = redis;
        }

        public async Task ExecuteAsync(InsertSessionIdCommand command)
        {
            var key = $"Session:{command.SessionId}";
            await redis.StringSetAsync(key, $"{command.UserRole}", DefaultExpire);
        }
    }
}