using AuthDb;
using AuthLibrary.Models;
using CommonLibrary;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

// TODO Library 레이어에서 Handler는 제공하지 말자
namespace AuthLibrary.Handlers;

public sealed record AddAccountCommand(
    string AccountId,
    string Password,
    ResSignUp.Types.AccountRoles AccountRole) : ICommand;

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
        var newRow = new Account()
        {
            AccountId = command.AccountId,
            Token = string.Empty,
            CreatedAt = time.Now,
            Role = command.AccountRole,
        };

        var passwordHasher = new PasswordHasher<Account>();
        newRow.Password = passwordHasher.HashPassword(newRow, Salt + command.Password);

        await dbContext.Accounts.AddAsync(newRow);
        await dbContext.SaveChangesAsync();

        return mapper.Map<AccountData>(newRow);
    }
}