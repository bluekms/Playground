using System;
using System.Threading.Tasks;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using StackExchange.Redis;

namespace AuthServer.Handlers.Session
{
    public sealed record InsertSessionCommand(
        string Token,
        UserRoles UserRole) : ICommand;

    public sealed class InsertSessionHandler : ICommandHandler<InsertSessionCommand>
    {
        private static TimeSpan DefaultExpire = new(0, 0, 5, 0);

        private readonly IDatabase redis;

        public InsertSessionHandler(IDatabase redis)
        {
            this.redis = redis;
        }

        public async Task ExecuteAsync(InsertSessionCommand command)
        {
            var key = $"Session:{command.Token}";
            await redis.StringSetAsync(key, $"{command.UserRole}", DefaultExpire);
        }
    }
}