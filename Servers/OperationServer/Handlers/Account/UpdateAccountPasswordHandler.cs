using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OperationServer.Handlers.Account;

public sealed record UpdateAccountPasswordCommand(string AccountId, string Password) : ICommand;

public class UpdateAccountPasswordHandler : ICommandHandler<UpdateAccountPasswordCommand>
{
    private const string Salt = "Playground.bluekms";

    private readonly AuthDbContext dbContext;

    public UpdateAccountPasswordHandler(AuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task ExecuteAsync(UpdateAccountPasswordCommand command)
    {
        var dbAccount = await dbContext.Accounts
            .SingleAsync(row => row.AccountId == command.AccountId);

        var password = Salt + command.Password;
        var passwordHasher = new PasswordHasher<AuthDb.Account>();

        dbAccount.Password = passwordHasher.HashPassword(dbAccount, password);

        await dbContext.SaveChangesAsync();
    }
}