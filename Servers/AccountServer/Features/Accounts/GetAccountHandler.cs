using System.Linq;
using System.Threading.Tasks;
using AccountServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Features.Accounts
{
    public sealed record GetAccountQuery(string AccountId) : IQuery;

    public sealed class GetAccountHandler : IQueryHandler<GetAccountQuery, AccountData?>
    {
        private readonly AuthContext _context;

        public GetAccountHandler(AuthContext context)
        {
            _context = context;
        }

        public async Task<AccountData?> QueryAsync(GetAccountQuery query)
        {
            var row = await _context.Accounts
                .Where(x => x.AccountId == query.AccountId)
                .SingleOrDefaultAsync();

            if (row == null)
            {
                return null;
            }

            return new(row.AccountId, row.SessionId, row.CreatedAt, row.Authority);
        }
    }
}