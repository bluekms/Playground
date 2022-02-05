using System;
using System.Threading.Tasks;
using AccountServer.Models;

namespace AccountServer.Handlers.Accounts
{
    public sealed record InsertAccountCommand(
        string AccountId,
        string Password,
        string SessionId,
        string Authority) : ICommand;

    public sealed class InsertAccountHandler : ICommandHandler<InsertAccountCommand, AccountData>
    {
        private readonly AuthContext _context;

        public InsertAccountHandler(AuthContext context)
        {
            _context = context;
        }

        public async Task<AccountData> ExecuteAsync(InsertAccountCommand command)
        {
            var newRow = new Account(
                command.AccountId,
                command.Password,
                command.SessionId,
                DateTime.UtcNow,
                command.Authority);

            await _context.Accounts.AddAsync(newRow);

            await _context.SaveChangesAsync();

            return new(newRow.AccountId, newRow.SessionId, newRow.CreatedAt, newRow.Authority);
        }
    }
}