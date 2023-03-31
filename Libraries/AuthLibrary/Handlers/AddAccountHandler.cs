using AuthDb;
using AuthLibrary.Models;
using CommonLibrary;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace AuthLibrary.Handlers;

public sealed record AddAccountCommand(
    string AccountId,
    string Password,
    AccountRoles AccountRole) : ICommand;

public class AddAccountHandler : ICommandHandler<AddAccountCommand, AccountData>
{
    private const string Salt = "Playground.bluekms";

    private readonly AuthDbContext dbContext;
    private readonly IMapper mapper;
    private readonly ITimeService time;

    public AddAccountHandler(
        AuthDbContext dbContext,
        IMapper mapper,
        ITimeService time)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.time = time;
    }

    public async Task<AccountData> ExecuteAsync(AddAccountCommand command)
    {
        var passwordHasher = new PasswordHasher<AuthDb.Account>();
        var account = new AuthDb.Account();
        var password = Salt + command.Password;
        var hashedPassword = passwordHasher.HashPassword(account, password);

        var newRow = new AuthDb.Account()
        {
            AccountId = command.AccountId,
            Password = hashedPassword,
            Token = string.Empty,
            CreatedAt = time.Now,
            Role = command.AccountRole,
        };

        await dbContext.Accounts.AddAsync(newRow);
        await dbContext.SaveChangesAsync();

        return mapper.Map<AccountData>(newRow);
    }
}