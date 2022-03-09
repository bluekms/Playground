using System;
using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using AuthServer.Models;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.Maintenance
{
    public sealed record AddMaintenanceCommand(
        DateTime Start,
        DateTime End,
        string Reason) : ICommand;

    public class AddMaintenanceHandler : ICommandHandler<AddMaintenanceCommand, MaintenanceData>
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public AddMaintenanceHandler(AuthContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<MaintenanceData> ExecuteAsync(AddMaintenanceCommand command)
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