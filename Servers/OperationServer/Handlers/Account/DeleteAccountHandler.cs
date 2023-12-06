using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;

namespace OperationServer.Handlers.Account;

public sealed record DeleteAccountCommand(string AccountId) : ICommand;

public sealed class DeleteAccountHandler : ICommandHandler<DeleteAccountCommand>
{
    private readonly AuthDbContext dbContext;

    public DeleteAccountHandler(AuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task ExecuteAsync(DeleteAccountCommand command)
    {
        await dbContext.Accounts
            .Where(row => row.AccountId == command.AccountId)
            .ExecuteDeleteAsync();

        await dbContext.SaveChangesAsync();
    }
}