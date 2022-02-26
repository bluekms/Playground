using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountServer.Models;
using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Handlers.World
{
    public sealed record GetWorldListQuery(ServerRoles Role) : IQuery;

    public sealed class GetWorldListHandler : IQueryHandler<GetWorldListQuery, List<ServerData>>
    {
        private readonly AuthContext context;
        private readonly ITimeService time;
        private readonly IMapper mapper;

        public GetWorldListHandler(AuthContext context, ITimeService time, IMapper mapper)
        {
            this.context = context;
            this.time = time;
            this.mapper = mapper;
        }

        public async Task<List<ServerData>> QueryAsync(GetWorldListQuery query)
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