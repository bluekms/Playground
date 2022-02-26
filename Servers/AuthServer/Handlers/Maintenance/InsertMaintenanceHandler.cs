using System;
using System.Threading.Tasks;
using AuthDb;
using AuthServer.Models;
using CommonLibrary.Handlers;
using MapsterMapper;

namespace AuthServer.Handlers.Maintenance
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