using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.Maintenance
{
    public sealed record IsMaintenanceTimeQuery() : IQuery;

    public class IsMaintenanceTimeHandler : IQueryHandler<IsMaintenanceTimeQuery, bool>
    {
        private readonly ITimeService timeService;
        private readonly AuthContext authContext;

        public IsMaintenanceTimeHandler(ITimeService timeService, AuthContext authContext)
        {
            this.timeService = timeService;
            this.authContext = authContext;
        }

        public async Task<bool> QueryAsync(IsMaintenanceTimeQuery query)
        {
            return await authContext.Maintenance
                .Where(x => x.Start <= timeService.Now)
                .Where(x => timeService.Now <= x.End)
                .AnyAsync();
        }
    }
}