using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OperationServer.Handlers.Account;

public sealed record UpdateAccountPasswordCommand(string AccountId, string Password) : ICommand;

public class UpdateAccountPasswordHandler : ICommandHandler<UpdateAccountPasswordCommand>
{
    private readonly AuthDbContext dbContext;
    private readonly string salt;

    public UpdateAccountPasswordHandler(AuthDbContext dbContext, string salt)
    {
        this.dbContext = dbContext;
        this.salt = salt;
    }

    public async Task ExecuteAsync(UpdateAccountPasswordCommand command)
    {
        var dbAccount = await dbContext.Accounts
            .SingleAsync(row => row.AccountId == command.AccountId);

        var passwordHasher = new PasswordHasher<AuthDb.Account>();
        dbAccount.Password = passwordHasher.HashPassword(dbAccount, salt + command.Password);

        await dbContext.SaveChangesAsync();
    }
}
