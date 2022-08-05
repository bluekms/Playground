using AuthLibrary.Extensions.Authentication;
using AuthServer.Handlers.Maintenance;
using AuthServer.Models;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

[ApiController]
public class AddMaintenanceController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IRuleChecker<AddMaintenanceRule> rule;
    private readonly ICommandHandler<AddMaintenanceCommand, MaintenanceData> addMaintenance;

    public AddMaintenanceController(
        IMapper mapper,
        IRuleChecker<AddMaintenanceRule> rule,
        ICommandHandler<AddMaintenanceCommand, MaintenanceData> addMaintenance)
    {
        this.mapper = mapper;
        this.rule = rule;
        this.addMaintenance = addMaintenance;
    }

    [HttpPost]
    [Route("Auth/Maintenance/Add")]
    [Authorize(AuthenticationSchemes = CredentialAuthenticationSchemeOptions.Name)]
    public async Task<ActionResult<Returns>> AddMaintenance([FromBody] Arguments args)
    {
        await rule.CheckAsync(new(args.Start, args.End, args.Reason));
        var newData = await addMaintenance.ExecuteAsync(new(args.Start, args.End, args.Reason));
        return mapper.Map<Returns>(newData);
    }

    public sealed record Arguments(DateTime Start, DateTime End, string Reason);

    public sealed record Returns(long Id, DateTime Start, DateTime End, string Reason);
}