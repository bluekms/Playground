using System;
using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace AuthServer.Handlers.Session
{
    public sealed record GetUserRoleQuery(string Token) : IQuery;

    public class GetUserRoleHandler : IQueryHandler<GetUserRoleQuery, UserRoles>
    {
        private readonly IDatabase redis;

        public GetUserRoleHandler(IDatabase redis)
        {
            this.redis = redis;
        }

        public async Task<UserRoles> QueryAsync(GetUserRoleQuery query)
        {
            var key = $"Session:{query.Token}";
            var userRole = await redis.StringGetAsync(key);

            return Enum.Parse<UserRoles>(userRole.ToString());
        }
    }
}