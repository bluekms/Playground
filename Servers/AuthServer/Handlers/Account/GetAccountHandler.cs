using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using AuthLibrary.Models;
using AuthServer.Models;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.Account
{
    public sealed record GetAccountQuery(string AccountId) : IQuery;

    public sealed class GetAccountHandler : IQueryHandler<GetAccountQuery, AccountData?>
    {
        private readonly AuthDbContext dbContext;
        private readonly IMapper mapper;

        public GetAccountHandler(AuthDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<AccountData?> QueryAsync(GetAccountQuery query, CancellationToken cancellationToken)
        {
            var row = await dbContext.Accounts
                .Where(x => x.AccountId == query.AccountId)
                .SingleOrDefaultAsync(cancellationToken);

            if (row == null)
            {
                return null;
            }

            return mapper.Map<AccountData>(row);
        }
    }
}
