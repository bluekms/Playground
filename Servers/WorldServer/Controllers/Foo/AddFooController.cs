using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Feature.Session;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary;
using WorldServer.Handlers.Foo;

namespace WorldServer.Controllers.Foo;

[ApiController]
public sealed class AddFooController : ControllerBase
{
    private readonly ICommandHandler<AddFooCommand> addFoo;
    private readonly StaticDataContext staticData;

    public AddFooController(
        ICommandHandler<AddFooCommand> addFoo,
        StaticDataContext staticData)
    {
        this.addFoo = addFoo;
        this.staticData = staticData;
    }

    [HttpPost]
    [Route("World/Foo/Add")]
    [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = "ServiceApi")]
    public async Task<ActionResult<string>> HandleAsync([FromBody] ArgumentData args, SessionData session, CancellationToken cancellationToken)
    {
        await addFoo.ExecuteAsync(new(args.Data));

        var id = int.Parse(args.Data);
        var record = await staticData.TypeTestTable.SingleAsync(x => x.Id == id);

        return $"Ok";
    }

    public sealed record ArgumentData(string Data);
}