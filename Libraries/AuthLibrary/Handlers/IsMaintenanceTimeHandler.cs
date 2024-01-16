using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;

namespace AuthLibrary.Handlers;

public sealed record IsMaintenanceTimeQuery() : IQuery;

public class IsMaintenanceTimeHandler : IQueryHandler<IsMaintenanceTimeQuery, bool>
{
    private readonly ITimeService timeService;
    private readonly AuthDbContext authDbContext;

    public IsMaintenanceTimeHandler(ITimeService timeService, AuthDbContext authDbContext)
    {
        this.timeService = timeService;
        this.authDbContext = authDbContext;
    }

    public async Task<bool> QueryAsync(IsMaintenanceTimeQuery query, CancellationToken cancellationToken)
    {
        return await authDbContext.Maintenances
            .Where(x => x.Start <= timeService.Now)
            .Where(x => timeService.Now <= x.End)
            .AnyAsync(cancellationToken);
    }
}
