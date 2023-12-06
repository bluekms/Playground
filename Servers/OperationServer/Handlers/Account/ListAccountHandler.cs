using AuthDb;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using OperationServer.Pages.Accounts.ViewModels;

namespace OperationServer.Handlers.Account;

public sealed record ListAccountQuery : IQuery;

public class ListAccountHandler : IQueryHandler<ListAccountQuery, List<AccountVM>>
{
    private readonly AuthDbContext dbContext;
    private readonly IMapper mapper;

    public ListAccountHandler(AuthDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<List<AccountVM>> QueryAsync(ListAccountQuery query, CancellationToken cancellationToken)
    {
        var dbAccounts = await dbContext.Accounts.ToListAsync(cancellationToken);

        return mapper.Map<List<AccountVM>>(dbAccounts);
    }
}