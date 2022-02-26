using System;
using System.Threading.Tasks;
using AccountServer.Extensions.Authentication;
using AccountServer.Handlers.Server;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountServer.Controllers
{
    [ApiController]
    public class AddServerController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICommandHandler<UpsertServerCommand> upsertServer;

        public AddServerController(IMapper mapper, ICommandHandler<UpsertServerCommand> upsertServer)
        {
            this.mapper = mapper;
            this.upsertServer = upsertServer;
        }

        [HttpPost, Route("Auth/Server/Add")]
        [Authorize(AuthenticationSchemes = CredentialAuthenticationSchemeOptions.Name)]
        public async Task<ActionResult> AddServer([FromBody] Arguments args)
        {
            var command = mapper.Map<UpsertServerCommand>(args);
            await upsertServer.ExecuteAsync(command);

            return Ok();
        }

        public sealed record Arguments(string Name, ServerRoles Role, string Address, DateTime ExpireAt, string Description);
    }
}