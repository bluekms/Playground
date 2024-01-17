using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OperationServer.Handlers.Account;

public sealed record UpdatePasswordCommand(string AccountId, string Password) : ICommand;

public class UpdatePasswordHandler : ICommandHandler<UpdatePasswordCommand>
{
    private readonly ITimeService timeService;
    private readonly AuthDbContext dbContext;

    public UpdatePasswordHandler(ITimeService timeService, AuthDbContext dbContext)
    {
        this.timeService = timeService;
        this.dbContext = dbContext;
    }

    public async Task ExecuteAsync(UpdatePasswordCommand command)
    {
        var accountRow = await dbContext.Accounts
            .Where(x => x.AccountId == command.AccountId)
            .SingleAsync();

        var passwordRow = await dbContext.Passwords
            .Where(x => x.AccountId == command.AccountId)
            .SingleAsync();

        var passwordHasher = new PasswordHasher<AuthDb.Account>();
        passwordRow.UpdatedAt = timeService.Now;
        passwordRow.AccountPassword = passwordHasher.HashPassword(accountRow, command.Password);

        await dbContext.SaveChangesAsync();
    }
}
