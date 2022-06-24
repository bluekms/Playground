using System;
using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using AuthServer.Models;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.Session
{
    public sealed record UpdateSessionCommand(string AccountId, string Token) : ICommand;

    public sealed class UpdateSessionHandler : ICommandHandler<UpdateSessionCommand, AccountData>
    {
        private readonly AuthDbContext dbContext;
        private readonly IMapper mapper;

        public UpdateSessionHandler(AuthDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<AccountData> ExecuteAsync(UpdateSessionCommand command)
        {
            var row = await dbContext.Accounts
                .Where(x => x.AccountId == command.AccountId)
                .SingleOrDefaultAsync();

            if (row == null)
            {
                throw new NullReferenceException(nameof(command.AccountId));
            }

            row.Token = command.Token;
            await dbContext.SaveChangesAsync();

            return mapper.Map<AccountData>(row);
        }
    }
}
