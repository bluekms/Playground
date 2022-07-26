using AuthLibrary.Extensions.Authentication;
using AuthServer.Handlers.Server;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

[ApiController]
public class RegisterServerController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ICommandHandler<UpsertServerCommand> upsertServer;

    public RegisterServerController(IMapper mapper, ICommandHandler<UpsertServerCommand> upsertServer)
    {
        this.mapper = mapper;
        this.upsertServer = upsertServer;
    }

    [HttpPost]
    [Route("Auth/Server/Register")]
    [Authorize(AuthenticationSchemes = CredentialAuthenticationSchemeOptions.Name)]
    public async Task<ActionResult> RegisterServer([FromBody] Arguments args)
    {
        var command = mapper.Map<UpsertServerCommand>(args);
        await upsertServer.ExecuteAsync(command);

        return Ok();
    }

    public sealed record Arguments(string Name, ServerRoles Role, string Address, string Description, long ExpireSec);
}