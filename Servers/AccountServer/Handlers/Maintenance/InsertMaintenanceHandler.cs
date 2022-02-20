using System;
using System.Threading.Tasks;
using AccountServer.Models;
using AuthDb;
using CommonLibrary.Handlers;
using MapsterMapper;

namespace AccountServer.Handlers.Maintenance
{
    public sealed record InsertMaintenanceCommand(
        DateTime Start,
        DateTime End,
        string Reason) : ICommand;

    public class InsertMaintenanceHandler : ICommandHandler<InsertMaintenanceCommand, MaintenanceData>
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public InsertMaintenanceHandler(AuthContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<MaintenanceData> ExecuteAsync(InsertMaintenanceCommand command)
        {
            var newRow = new AuthDb.Maintenance()
            {
                Start = command.Start,
                End = command.End,
                Reason = command.Reason,
            };

            await context.Maintenance.AddAsync(newRow);

            await context.SaveChangesAsync();

            return mapper.Map<MaintenanceData>(newRow);
        }
    }
}