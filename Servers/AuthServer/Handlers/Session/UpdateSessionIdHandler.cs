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

    public sealed class UpdateAccountHandler : ICommandHandler<UpdateSessionIdCommand, AccountData>
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public UpdateAccountHandler(AuthContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<AccountData> ExecuteAsync(UpdateSessionIdCommand command)
        {
            var row = await context.Accounts
                .Where(x => x.AccountId == command.AccountId)
                .SingleOrDefaultAsync();

            if (row == null)
            {
                throw new NullReferenceException(nameof(command.AccountId));
            }

            row.Token = command.SessionId;
            await context.SaveChangesAsync();

            return mapper.Map<AccountData>(row);
        }
    }
}
