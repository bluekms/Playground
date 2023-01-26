using AuthLibrary.Feature.Session;
using CommonLibrary.Handlers;
using CommonLibrary.Models;

namespace AuthLibrary.Handlers;

public sealed record GetAccountRoleQuery(string Token) : IQuery;

public class GetUserRoleHandler : IQueryHandler<GetAccountRoleQuery, AccountRoles?>
{
    private readonly SessionStore sessionStore;

    public GetUserRoleHandler(SessionStore sessionStore)
    {
        this.sessionStore = sessionStore;
    }

    public async Task<AccountRoles?> QueryAsync(GetAccountRoleQuery query, CancellationToken cancellationToken)
    {
        var sessionData = await sessionStore.GetAsync(query.Token, cancellationToken);
        return Enum.Parse<AccountRoles>(sessionData.Roles.ToString());
    }
}