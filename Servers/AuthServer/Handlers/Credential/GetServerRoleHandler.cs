using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Handlers.Credential
{
    public sealed record GetServerRoleQuery(string Token) : IQuery;

    public class GetServerRoleHandler : IQueryHandler<GetServerRoleQuery, ServerRoles>
    {
        private readonly AuthContext context;

        public GetServerRoleHandler(AuthContext context)
        {
            this.context = context;
        }

        public async Task<ServerRoles> QueryAsync(GetServerRoleQuery query)
        {
            return await context.Credentials
                .Where(x => x.Token == query.Token)
                .Select(x => x.Role)
                .SingleAsync();
        }
    }
}