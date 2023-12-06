using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Identity;

namespace OperationServer.Handlers.Account;

public sealed record CreateAccountCommand(
    string AccountId,
    string Password,
    ResSignUp.Types.AccountRoles Role) : ICommand;

public sealed class CreateAccountHandler : ICommandHandler<CreateAccountCommand>
{
    private const string Salt = "Playground.bluekms";

    private readonly AuthDbContext dbContext;
    private readonly ITimeService timeService;

    public CreateAccountHandler(AuthDbContext dbContext, ITimeService timeService)
    {
        this.dbContext = dbContext;
        this.timeService = timeService;
    }

    public async Task ExecuteAsync(CreateAccountCommand command)
    {
        var newAccount = new AuthDb.Account
        {
            AccountId = command.AccountId,
            CreatedAt = timeService.Now,
            Token = string.Empty,
            Role = command.Role,
        };

        var passwordHasher = new PasswordHasher<AuthDb.Account>();
        newAccount.Password = passwordHasher.HashPassword(newAccount, Salt + command.Password);

        await dbContext.Accounts.AddAsync(newAccount);
        await dbContext.SaveChangesAsync();
    }
}