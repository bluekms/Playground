using AuthDb;
using AuthLibrary.Models;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AuthServer.Handlers.Session
{
    public sealed record UpdateSessionCommand(string AccountId) : ICommand;

    public sealed class UpdateSessionHandler : ICommandHandler<UpdateSessionCommand, AccountData>
    {
        private static readonly string SessionPrefix = "Session";

        private readonly IDatabase redis;
        private readonly AuthDbContext dbContext;
        private readonly IMapper mapper;

        public UpdateSessionHandler(
            IConnectionMultiplexer multiplexer,
            AuthDbContext dbContext,
            IMapper mapper)
        {
            redis = multiplexer.GetDatabase();
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<AccountData> ExecuteAsync(UpdateSessionCommand command)
        {
            var account = await dbContext.Accounts
                .Where(x => x.AccountId == command.AccountId)
                .SingleOrDefaultAsync();

            if (account == null)
            {
                throw new NullReferenceException(nameof(command.AccountId));
            }

            await redis.KeyDeleteAsync($"{SessionPrefix}:{account.Token}");
            account.Token = Guid.NewGuid().ToString();
            await dbContext.SaveChangesAsync();

            return mapper.Map<AccountData>(account);
        }
    }
}
