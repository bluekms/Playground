using AuthLibrary.Extensions.Authentication;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldServer.Handlers.Foo;

namespace WorldServer.Controllers.Foo;

[ApiController]
public sealed class AddFooController : ControllerBase
{
    private readonly ICommandHandler<AddFooCommand> addFoo;

    public AddFooController(ICommandHandler<AddFooCommand> addFoo)
    {
        this.addFoo = addFoo;
    }

    [HttpPost]
    [Route("World/Foo/Add")]
    [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = "ServiceApi")]
    public async Task<ActionResult<string>> HandleAsync([FromBody] ArgumentData args)
    {
        await addFoo.ExecuteAsync(new(args.Data));

        return "Ok";
    }

    public sealed record ArgumentData(string Data);
}