using System;
using System.Threading.Tasks;
using AccountServer.Models;
using MapsterMapper;

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
        private readonly IMapper _mapper;

        public InsertAccountHandler(
            AuthContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountData> ExecuteAsync(InsertAccountCommand command)
        {
            var newRow = new AuthContext.Account(
                command.AccountId,
                command.Password,
                command.SessionId,
                DateTime.UtcNow,
                command.Authority);

            await _context.Accounts.AddAsync(newRow);

            await _context.SaveChangesAsync();

            return _mapper.Map<AccountData>(newRow);
        }
    }
}