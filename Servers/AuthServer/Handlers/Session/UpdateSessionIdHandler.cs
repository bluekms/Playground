using AuthDb;
using AuthLibrary.Models;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AuthServer.Handlers.Session
{
    public sealed record UpdateSessionCommand(string AccountId, string Token) : ICommand;

    public sealed class UpdateSessionHandler : ICommandHandler<UpdateSessionCommand, AccountData>
    {
        private static readonly TimeSpan DefaultExpire = new(0, 0, 5, 0);

        private readonly AuthDbContext dbContext;
        private readonly IDatabase redis;
        private readonly IMapper mapper;

        public UpdateSessionHandler(
            AuthDbContext dbContext,
            IDatabase redis,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.redis = redis;
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

            await redis.KeyDeleteAsync($"Session:{account.Token}");

            account.Token = command.Token;

            await redis.StringSetAsync($"Session:{command.Token}", $"{account.Role}", DefaultExpire);

            await dbContext.SaveChangesAsync();
            return mapper.Map<AccountData>(account);
        }
    }
}
