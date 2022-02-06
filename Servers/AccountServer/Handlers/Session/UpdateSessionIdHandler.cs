using System;
using System.Linq;
using System.Threading.Tasks;
using AccountServer.Models;
using AuthDb;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Handlers.Session
{
    public sealed record UpdateSessionIdCommand(string AccountId, string SessionId) : ICommand;

    public class UpdateAccountHandler : ICommandHandler<UpdateSessionIdCommand, AccountData>
    {
        private readonly AuthContext _context;
        private readonly IMapper _mapper;

        public UpdateAccountHandler(AuthContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountData> ExecuteAsync(UpdateSessionIdCommand command)
        {
            var row = await _context.Accounts
                .Where(x => x.AccountId == command.AccountId)
                .SingleOrDefaultAsync();

            if (row == null)
            {
                throw new NullReferenceException(nameof(command.AccountId));
            }

            row.SessionId = command.SessionId;
            await _context.SaveChangesAsync();

            return _mapper.Map<AccountData>(row);
        }
    }
}
