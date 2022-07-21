using AuthDb;
using AuthServer.Models;
using CommonLibrary;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using MapsterMapper;

namespace AuthServer.Handlers.Account;

public sealed record AddAccountCommand(
    string AccountId,
    string Password,
    UserRoles UserRole) : ICommand;

public sealed class AddAccountHandler : ICommandHandler<AddAccountCommand, AccountData>
{
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
        var newRow = new AuthDb.Account()
        {
            AccountId = command.AccountId,
            Password = command.Password,
            Token = string.Empty,
            CreatedAt = time.Now,
            Role = command.UserRole,
        };

        await dbContext.Accounts.AddAsync(newRow);
        await dbContext.SaveChangesAsync();

        return mapper.Map<AccountData>(newRow);
    }
}