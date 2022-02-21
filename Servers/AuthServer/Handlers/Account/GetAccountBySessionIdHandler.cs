using System.Linq;
using System.Threading.Tasks;
using AccountServer.Models;
using AuthDb;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Handlers.Account
{
    public sealed record GetAccountBySessionIdQuery(string SessionId) : IQuery;

    public class GetAccountBySessionIdHandler : IQueryHandler<GetAccountBySessionIdQuery, AccountData?>
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public GetAccountBySessionIdHandler(AuthContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<AccountData?> QueryAsync(GetAccountBySessionIdQuery query)
        {
            var row = await context.Accounts
                .Where(x => x.SessionId == query.SessionId)
                .SingleOrDefaultAsync();

            if (row == null)
            {
                return null;
            }

            return mapper.Map<AccountData>(row);
        }
    }
}