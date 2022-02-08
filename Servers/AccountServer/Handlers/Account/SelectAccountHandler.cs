using System.Linq;
using System.Threading.Tasks;
using AccountServer.Models;
using AuthDb;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Handlers.Account
{
    public sealed record SelectAccountQuery(string AccountId) : IQuery;

    public sealed class SelectAccountHandler : IQueryHandler<SelectAccountQuery, AccountData?>
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public SelectAccountHandler(AuthContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<AccountData?> QueryAsync(SelectAccountQuery query)
        {
            var row = await context.Accounts
                .Where(x => x.AccountId == query.AccountId)
                .SingleOrDefaultAsync();

            if (row == null)
            {
                return null;
            }

            return mapper.Map<AccountData>(row);
        }
    }
}
