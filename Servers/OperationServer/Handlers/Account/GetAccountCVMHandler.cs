using AuthDb;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using OperationServer.Pages.Accounts.ViewModels;

namespace OperationServer.Handlers.Account;

public sealed record GetAccountQuery(string AccountId) : IQuery;

public sealed class GetAccountVMHandler : IQueryHandler<GetAccountQuery, AccountVM>
{
    // TODO Use ReadonlyContext
    private readonly AuthDbContext dbContext;
    private readonly IMapper mapper;

    public GetAccountVMHandler(AuthDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<AccountVM> QueryAsync(GetAccountQuery query, CancellationToken cancellationToken)
    {
        var dbAccount = await dbContext.Accounts
            .SingleAsync(row => row.AccountId == query.AccountId, cancellationToken);

        return mapper.Map<AccountVM>(dbAccount);
    }
}

public sealed class GetAccountUpdatePasswordVMHandler : IQueryHandler<GetAccountQuery, AccountUpdatePasswordVM>
{
    private readonly AuthDbContext dbContext;

    public GetAccountUpdatePasswordVMHandler(AuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<AccountUpdatePasswordVM> QueryAsync(GetAccountQuery query, CancellationToken cancellationToken)
    {
        var dbAccount = await dbContext.Accounts
            .SingleAsync(row => row.AccountId == query.AccountId, cancellationToken);

        return new AccountUpdatePasswordVM
        {
            AccountId = dbAccount.AccountId,
            Password = string.Empty,
            ConfirmPassword = string.Empty,
        };
    }
}

public sealed class GetAccountUpdateRoleVMHandler : IQueryHandler<GetAccountQuery, AccountUpdateRoleVM>
{
    private readonly AuthDbContext dbContext;

    public GetAccountUpdateRoleVMHandler(AuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<AccountUpdateRoleVM> QueryAsync(GetAccountQuery query, CancellationToken cancellationToken)
    {
        var dbAccount = await dbContext.Accounts
            .SingleAsync(row => row.AccountId == query.AccountId, cancellationToken);

        return new AccountUpdateRoleVM
        {
            AccountId = dbAccount.AccountId,
            Role = dbAccount.Role,
        };
    }
}
