using AuthDb;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthLibrary.Handlers;

public sealed record GetServerRoleQuery(string Token) : IQuery;

public class GetServerRoleHandler : IQueryHandler<GetServerRoleQuery, ServerRoles>
{
    private readonly AuthDbContext dbContext;

    public GetServerRoleHandler(AuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ServerRoles> QueryAsync(GetServerRoleQuery query, CancellationToken cancellationToken)
    {
        return await dbContext.ServerRoles
            .Where(row => row.Token == query.Token)
            .Select(row => row.Role)
            .SingleAsync(cancellationToken);
    }
}