using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.Credential
{
    public sealed record GetServerRoleQuery(string Token) : IQuery;

    public class GetServerRoleHandler : IQueryHandler<GetServerRoleQuery, ServerRoles>
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
}