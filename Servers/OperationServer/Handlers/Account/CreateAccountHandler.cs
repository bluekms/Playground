using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Identity;

namespace OperationServer.Handlers.Account;

public sealed record CreateAccountCommand(
    string AccountId,
    string Password,
    ResSignUp.Types.AccountRoles AccountRole) : ICommand;

public class CreateAccountHandler : ICommandHandler<CreateAccountCommand>
{
    private readonly ITimeService timeService;
    private readonly AuthDbContext dbContext;

    public CreateAccountHandler(
        ITimeService timeService,
        AuthDbContext dbContext)
    {
        this.timeService = timeService;
        this.dbContext = dbContext;
    }

    public async Task ExecuteAsync(CreateAccountCommand command)
    {
        var newAccount = new AuthDb.Account
        {
            AccountId = command.AccountId,
            Token = string.Empty,
            CreatedAt = timeService.Now,
            Role = command.AccountRole,
        };

        var passwordHasher = new PasswordHasher<AuthDb.Account>();
        var newPassword = new Password
        {
            AccountId = command.AccountId,
            UpdatedAt = timeService.Now,
            AccountPassword = passwordHasher.HashPassword(newAccount, command.Password),
        };

        await dbContext.Accounts.AddAsync(newAccount);
        await dbContext.Passwords.AddAsync(newPassword);

        await dbContext.SaveChangesAsync();
    }
}
