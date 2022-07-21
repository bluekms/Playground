using CommonLibrary.Handlers;
using CommonLibrary.Models;
using StackExchange.Redis;

namespace AuthLibrary.Handlers;

public sealed record GetUserRoleQuery(string Token) : IQuery;

public class GetUserRoleHandler : IQueryHandler<GetUserRoleQuery, UserRoles?>
{
    private readonly IDatabase redis;

    public GetUserRoleHandler(IDatabase redis)
    {
        this.redis = redis;
    }

    public async Task<UserRoles?> QueryAsync(GetUserRoleQuery query)
    {
        var key = $"Session:{query.Token}";
        var userRole = await redis.StringGetAsync(key);

        if (!userRole.HasValue)
        {
            return null;
        }

        return Enum.Parse<UserRoles>(userRole.ToString());
    }   
}