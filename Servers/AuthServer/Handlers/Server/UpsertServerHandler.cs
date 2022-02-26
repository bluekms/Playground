using System;
using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using MapsterMapper;

namespace AccountServer.Handlers.Server
{
    public sealed record UpsertServerCommand(
        string Name,
        ServerRoles Role,
        string Address,
        DateTime ExpireAt,
        string Description) : ICommand;

    public class UpsertServerHandler : ICommandHandler<UpsertServerCommand>
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public UpsertServerHandler(AuthContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task ExecuteAsync(UpsertServerCommand command)
        {
            var row = await context.Servers
                .FindAsync(new object[] { command.Name });

            if (row == null)
            {
                row = mapper.Map<AuthDb.Server>(command);

                await context.Servers.AddAsync(row);
            }
            else
            {
                row.Role = command.Role;
                row.Address = command.Address;
                row.ExpireAt = command.ExpireAt;
                row.Description = command.Description;
            }

            await context.SaveChangesAsync();
        }
    }
}