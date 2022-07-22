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

    public sealed class AddMaintenanceHandler : ICommandHandler<AddMaintenanceCommand, MaintenanceData>
    {
        private readonly AuthDbContext dbContext;
        private readonly IMapper mapper;

        public AddMaintenanceHandler(AuthDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
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
            await dbContext.Maintenance.AddAsync(newRow);
            await dbContext.SaveChangesAsync();
            return mapper.Map<MaintenanceData>(newRow);
        }
    }
}