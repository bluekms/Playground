using System;
using System.Threading.Tasks;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using StackExchange.Redis;

namespace AuthServer.Handlers.Session
{
    public sealed record AddSessionCommand(
        string Token,
        UserRoles UserRole) : ICommand;

    public sealed class AddSessionHandler : ICommandHandler<AddSessionCommand>
    {
        private static TimeSpan DefaultExpire = new(0, 0, 5, 0);

        private readonly IDatabase redis;

        public AddSessionHandler(IDatabase redis)
        {
            this.redis = redis;
        }

        public async Task ExecuteAsync(AddSessionCommand command)
        {
            var key = $"Session:{command.Token}";
            await redis.StringSetAsync(key, $"{command.UserRole}", DefaultExpire);
        }
    }
}