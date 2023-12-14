using AuthDb;
using AuthServer.Models;
using CommonLibrary;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.World;

public sealed record GetServerListQuery(ServerRoles Role) : IQuery;

public sealed class GetServerListHandler : IQueryHandler<GetServerListQuery, List<ServerData>>
{
    private readonly ReadOnlyAuthDbContext dbContext;
    private readonly ITimeService timeService;
    private readonly IMapper mapper;

    public GetServerListHandler(ReadOnlyAuthDbContext dbContext, ITimeService timeService, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.timeService = timeService;
        this.mapper = mapper;
    }

    public async Task<List<ServerData>> QueryAsync(GetServerListQuery query, CancellationToken cancellationToken)
    {
        var rows = await dbContext.Servers
            .Where(x => x.Role == query.Role)
            .Where(x => x.ExpireAt > timeService.Now)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<ServerData>>(rows.Where(x => timeService.Now < x.ExpireAt));
    }
}
