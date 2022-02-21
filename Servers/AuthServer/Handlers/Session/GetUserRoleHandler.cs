using System;
using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace AccountServer.Handlers.Session
{
    public sealed record GetUserRoleQuery(string SessionId) : IQuery;

    public class GetUserRoleHandler : IQueryHandler<GetUserRoleQuery, string>
    {
        private readonly IDatabase redis;

        public GetUserRoleHandler(IDatabase redis)
        {
            this.redis = redis;
        }

        public async Task<string> QueryAsync(GetUserRoleQuery query)
        {
            var key = $"Session:{query.SessionId}";
            return await redis.StringGetAsync(key);
        }
    }
}