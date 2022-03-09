using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using AuthServer.Models;
using CommonLibrary;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.World
{
    public sealed record GetServerListQuery(ServerRoles Role) : IQuery;

    public sealed class GetServerListHandler : IQueryHandler<GetServerListQuery, List<ServerData>>
    {
        private readonly AuthContext context;
        private readonly ITimeService time;
        private readonly IMapper mapper;

        public GetServerListHandler(AuthContext context, ITimeService time, IMapper mapper)
        {
            this.context = context;
            this.time = time;
            this.mapper = mapper;
        }

        public async Task<List<ServerData>> QueryAsync(GetServerListQuery query)
        {
            var rows = await context.Servers
                .Where(x => x.Role == query.Role)
                .ToListAsync();

            context.Servers.RemoveRange(rows.Where(x => x.ExpireAt <= time.Now));
            await context.SaveChangesAsync();

            return mapper.Map<List<ServerData>>(rows.Where(x => time.Now < x.ExpireAt));
        }
    }
}