using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace OperationServer.Handlers.Account;

public sealed record UpdateSessionCommand(string AccountId) : ICommand;

public sealed record UpdateSessionCommandResult(string Token, ResSignUp.Types.AccountRoles Role);

public class UpdateSessionHandler : ICommandHandler<UpdateSessionCommand, UpdateSessionCommandResult>
{
    private readonly IConnectionMultiplexer multiplexer;
    private readonly AuthDbContext dbContext;

    public UpdateSessionHandler(
        IConnectionMultiplexer multiplexer,
        AuthDbContext dbContext)
    {
        this.multiplexer = multiplexer;
        this.dbContext = dbContext;
    }

    public async Task<UpdateSessionCommandResult> ExecuteAsync(UpdateSessionCommand command)
    {
        var account = await dbContext.Accounts
            .Where(x => x.AccountId == command.AccountId)
            .SingleAsync();

        var redis = multiplexer.GetDatabase();
        await redis.KeyDeleteAsync(account.Token);

        account.Token = Guid.NewGuid().ToString();
        await dbContext.SaveChangesAsync();

        return new(account.Token, account.Role);
    }
}
