using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Handlers.Account;

public sealed record SignUpCommand(
    string Salt,
    string AccountId,
    string Password,
    ResSignUp.Types.AccountRoles AccountRole) : ICommand;

public sealed class SignUpHandler : ICommandHandler<SignUpCommand>
{
    private readonly ITimeService timeService;
    private readonly AuthDbContext dbContext;

    public SignUpHandler(
        ITimeService timeService,
        AuthDbContext dbContext)
    {
        this.timeService = timeService;
        this.dbContext = dbContext;
    }

    public async Task ExecuteAsync(SignUpCommand command)
    {
        var newAccount = new AuthDb.Account
        {
            AccountId = command.AccountId,
            Token = string.Empty,
            CreatedAt = timeService.Now,
            Role = command.AccountRole,
        };

        var passwordHasher = new PasswordHasher<AuthDb.Account>();
        var password = command.Salt + command.Password;
        newAccount.Password = passwordHasher.HashPassword(newAccount, password);

        await dbContext.Accounts.AddAsync(newAccount);
        await dbContext.SaveChangesAsync();
    }
}
