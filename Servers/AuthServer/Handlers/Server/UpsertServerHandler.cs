using System;
using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using MapsterMapper;

namespace AuthServer.Handlers.Server
{
    public sealed record UpsertServerCommand(
        string Name,
        ServerRoles Role,
        string Address,
        string Description,
        long ExpireSec) : ICommand;

    public sealed class UpsertServerHandler : ICommandHandler<UpsertServerCommand>
    {
        private readonly AuthDbContext dbContext;
        private readonly ITimeService timeService;
        private readonly IMapper mapper;

        public UpsertServerHandler(
            AuthDbContext dbContext,
            ITimeService timeService,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.timeService = timeService;
            this.mapper = mapper;
        }

        public async Task ExecuteAsync(UpsertServerCommand command)
        {
            var row = await dbContext.Servers
                .FindAsync(command.Name);

            if (row == null)
            {
                row = new()
                {
                    Name = command.Name,
                    Role = command.Role,
                    Address = command.Address,
                    ExpireAt = timeService.Now.AddSeconds(command.ExpireSec),
                    Description = command.Description,
                };

                await dbContext.Servers.AddAsync(row);
            }
            else
            {
                row.Role = command.Role;
                row.Address = command.Address;
                row.ExpireAt = timeService.Now.AddSeconds(command.ExpireSec);
                row.Description = command.Description;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}