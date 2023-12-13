using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.Maintenance;

public sealed record AddMaintenanceRule(DateTime Start, DateTime End, string Reason) : IRule;

public sealed class CheckMaintenanceRule : IRuleChecker<AddMaintenanceRule>
{
    private readonly ReadOnlyAuthDbContext dbContext;

    public CheckMaintenanceRule(ReadOnlyAuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CheckAsync(AddMaintenanceRule rule, CancellationToken cancellationToken)
    {
        var row = await dbContext.Maintenance
            .Where(x => x.Start <= rule.Start)
            .Where(x => rule.Start <= x.End)
            .SingleOrDefaultAsync(cancellationToken);

        if (row != null)
        {
            throw new Exception($"Duplicate Start. {row.ToString()}");
        }

        row = await dbContext.Maintenance
            .Where(x => x.Start <= rule.End)
            .Where(x => rule.End <= x.End)
            .SingleOrDefaultAsync(cancellationToken);

        if (row != null)
        {
            throw new Exception($"Duplicate End. {@row}");
        }

        var exist = await dbContext.Maintenance
            .Where(x => rule.Start <= x.Start)
            .Where(x => x.End <= rule.End)
            .AnyAsync(cancellationToken);

        if (exist)
        {
            throw new Exception($"Duplicate Start and End. {@row}");
        }
    }
}
