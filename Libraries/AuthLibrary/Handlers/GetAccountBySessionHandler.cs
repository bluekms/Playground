using AuthDb;
using AuthLibrary.Models;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthLibrary.Handlers;

public sealed record GetAccountBySessionQuery(string Token) : IQuery;

public class GetAccountBySessionHandler : IQueryHandler<GetAccountBySessionQuery, AccountData?>
{
    private readonly AuthDbContext dbContext;
    private readonly IMapper mapper;

    public GetAccountBySessionHandler(AuthDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<AccountData?> QueryAsync(GetAccountBySessionQuery query, CancellationToken cancellationToken)
    {
        var row = await dbContext.Accounts
            .Where(x => x.Token == query.Token)
            .SingleOrDefaultAsync(cancellationToken);

        if (row == null)
        {
            return null;
        }

        return mapper.Map<AccountData>(row);
    }
}