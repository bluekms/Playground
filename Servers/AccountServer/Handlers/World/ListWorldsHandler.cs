using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountServer.Models;
using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Handlers.World
{
    public sealed record ListWorldsQuery(string WorldType) : IQuery;

    public sealed class ListWorldsHandler : IQueryHandler<ListWorldsQuery, List<WorldData>>
    {
        private readonly AuthContext context;
        private readonly ITimeService time;
        private readonly IMapper mapper;

        public ListWorldsHandler(AuthContext context, ITimeService time, IMapper mapper)
        {
            this.context = context;
            this.time = time;
            this.mapper = mapper;
        }

        public async Task<List<WorldData>> QueryAsync(ListWorldsQuery query)
        {
            var rows = await context.Worlds
                .Where(x => x.WorldType == query.WorldType)
                .ToListAsync();

            context.Worlds.RemoveRange(rows.Where(x => x.ExpireAt <= time.Now));
            await context.SaveChangesAsync();

            return mapper.Map<List<WorldData>>(rows.Where(x => time.Now < x.ExpireAt));
        }
    }
}