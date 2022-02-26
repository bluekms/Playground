using System.Linq;
using System.Threading.Tasks;
using AccountServer.Models;
using AuthDb;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Handlers.Account
{
    public sealed record GetAccountBySessionQuery(string Token) : IQuery;

    public class GetAccountBySessionHandler : IQueryHandler<GetAccountBySessionQuery, AccountData?>
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public GetAccountBySessionHandler(AuthContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<AccountData?> QueryAsync(GetAccountBySessionQuery query)
        {
            var row = await context.Accounts
                .Where(x => x.Token == query.Token)
                .SingleOrDefaultAsync();

            if (row == null)
            {
                return null;
            }

            return mapper.Map<AccountData>(row);
        }
    }
}