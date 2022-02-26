using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;

namespace AuthServer.Handlers.Maintenance
{
    public sealed record AddMaintenanceRule(DateTime Start, DateTime End, string Reason) : IRule;

    public sealed class CheckMaintenanceRule : IRuleChecker<AddMaintenanceRule>
    {
        private readonly AuthContext context;

        public CheckMaintenanceRule(AuthContext context)
        {
            this.context = context;
        }

        public async Task CheckAsync(AddMaintenanceRule rule)
        {
            var row = await context.Maintenance
                .Where(x => x.Start <= rule.Start)
                .Where(x => rule.Start <= x.End)
                .SingleOrDefaultAsync();

            if (row != null)
            {
                throw new Exception($"Duplicate Start. {row.ToString()}");
            }

            row = await context.Maintenance
                .Where(x => x.Start <= rule.End)
                .Where(x => rule.End <= x.End)
                .SingleOrDefaultAsync();

            if (row != null)
            {
                throw new Exception($"Duplicate End. {@row}");
            }

            var exist = await context.Maintenance
                .Where(x => rule.Start <= x.Start)
                .Where(x => x.End <= rule.End)
                .AnyAsync();

            if (exist)
            {
                throw new Exception($"Duplicate Start and End. {@row}");
            }
        }
    }
}