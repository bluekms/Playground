using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using AuthServer.Models;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.Account
{
    public sealed record GetAccountQuery(string AccountId) : IQuery;

    public sealed class GetAccountHandler : IQueryHandler<GetAccountQuery, AccountData?>
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public GetAccountHandler(AuthContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<AccountData?> QueryAsync(GetAccountQuery query)
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
