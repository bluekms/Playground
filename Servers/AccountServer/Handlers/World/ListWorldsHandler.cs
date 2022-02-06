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
        private readonly AuthContext _context;
        private readonly ITimeService _time;
        private readonly IMapper _mapper;

        public ListWorldsHandler(AuthContext context, ITimeService time, IMapper mapper)
        {
            _context = context;
            _time = time;
            _mapper = mapper;
        }

        public async Task<List<WorldData>> QueryAsync(ListWorldsQuery query)
        {
            var rows = await _context.Worlds
                .Where(x => x.WorldType == query.WorldType)
                .ToListAsync();

            _context.Worlds.RemoveRange(rows.Where(x => x.ExpireAt <= _time.Now));
            await _context.SaveChangesAsync();

            return _mapper.Map<List<WorldData>>(rows.Where(x => _time.Now < x.ExpireAt));
        }
    }
}