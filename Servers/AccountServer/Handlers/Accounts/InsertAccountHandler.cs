using System;
using System.Threading.Tasks;
using AccountServer.Models;
using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using MapsterMapper;

namespace AccountServer.Handlers.Accounts
{
    public sealed record InsertAccountCommand(
        string AccountId,
        string Password,
        string Authority) : ICommand;

    public sealed class InsertAccountHandler : ICommandHandler<InsertAccountCommand, AccountData>
    {
        private readonly AuthContext _context;
        private readonly IMapper _mapper;
        private readonly ITimeService _time;

        public InsertAccountHandler(
            AuthContext context,
            IMapper mapper,
            ITimeService time)
        {
            _context = context;
            _mapper = mapper;
            _time = time;
        }

        public async Task<AccountData> ExecuteAsync(InsertAccountCommand command)
        {
            var newRow = new AuthContext.Account()
            {
                AccountId = command.AccountId,
                Password = command.Password,
                SessionId = string.Empty,
                CreatedAt = _time.Now,
                Authority = command.Authority,
            };

            await _context.Accounts.AddAsync(newRow);
            await _context.SaveChangesAsync();

            return _mapper.Map<AccountData>(newRow);
        }
    }
}