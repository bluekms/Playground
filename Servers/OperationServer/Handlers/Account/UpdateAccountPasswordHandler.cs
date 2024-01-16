using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OperationServer.Handlers.Account;

public sealed record UpdateAccountPasswordCommand(string AccountId, string Password) : ICommand;

public class UpdateAccountPasswordHandler : ICommandHandler<UpdateAccountPasswordCommand>
{
    private readonly ITimeService timeService;
    private readonly AuthDbContext dbContext;

    public UpdateAccountPasswordHandler(
        ITimeService timeService,
        AuthDbContext dbContext)
    {
        this.timeService = timeService;
        this.dbContext = dbContext;
    }

    public async Task ExecuteAsync(UpdateAccountPasswordCommand command)
    {
        var accountRow = await dbContext.Accounts
            .SingleAsync(row => row.AccountId == command.AccountId);

        var passwordRow = await dbContext.Passwords
            .SingleAsync(row => row.AccountId == command.AccountId);

        var passwordHasher = new PasswordHasher<AuthDb.Account>();
        passwordRow.UpdatedAt = timeService.Now;
        passwordRow.AccountPassword = passwordHasher.HashPassword(accountRow, command.Password);

        await dbContext.SaveChangesAsync();
    }
}
