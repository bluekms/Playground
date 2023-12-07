using CommonLibrary.Handlers;
using StackExchange.Redis;

namespace AuthServer.Handlers.Session
{
    public sealed record DeleteSessionCommand(string Session) : ICommand;

    public sealed class DeleteSessionHandler : ICommandHandler<DeleteSessionCommand>
    {
        private readonly IDatabase redis;

        public DeleteSessionHandler(IConnectionMultiplexer multiplexer)
        {
            redis = multiplexer.GetDatabase();
        }

        public async Task ExecuteAsync(DeleteSessionCommand command)
        {
            var key = $"Session:{command.Session}";
            await redis.KeyDeleteAsync(key);
        }
    }
}
