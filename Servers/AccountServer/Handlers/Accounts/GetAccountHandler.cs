using System.Linq;
using System.Threading.Tasks;
using AccountServer.Models;
using AuthDb;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Handlers.Accounts
{
    public sealed record GetAccountQuery(string AccountId) : IQuery;

    public sealed class GetAccountHandler : IQueryHandler<GetAccountQuery, AccountData?>
    {
        private readonly AuthContext _context;
        private readonly IMapper _mapper;

        public GetAccountHandler(
            AuthContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            return _mapper.Map<AccountData>(row);
        }
    }
}