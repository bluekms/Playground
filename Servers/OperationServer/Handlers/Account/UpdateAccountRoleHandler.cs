using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;

namespace OperationServer.Handlers.Account;

public sealed record UpdateAccountRoleCommand(string AccountId, ResSignUp.Types.AccountRoles Role) : ICommand;

public class UpdateAccountRoleHandler : ICommandHandler<UpdateAccountRoleCommand>
{
    private readonly AuthDbContext dbContext;

    public UpdateAccountRoleHandler(AuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task ExecuteAsync(UpdateAccountRoleCommand command)
    {
        var dbAccount = await dbContext.Accounts
            .SingleAsync(row => row.AccountId == command.AccountId);

        dbAccount.Role = command.Role;

        await dbContext.SaveChangesAsync();
    }
}