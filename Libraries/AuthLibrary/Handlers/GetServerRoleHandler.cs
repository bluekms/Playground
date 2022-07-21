using AuthDb;
using CommonLibrary.Handlers;
using CommonLibrary.Models;

namespace AuthLibrary.Handlers;

public sealed record GetServerRoleQuery(string Token) : IQuery;

public class GetServerRoleHandler
{
    private readonly AuthDbContext dbContext;

    public GetServerRoleHandler(AuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<ServerRoles> QueryAsync(GetServerRoleQuery query)
    {
        throw new NotImplementedException();
    }
}