using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Handlers.Account;

public sealed record SignUpCommand(
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
