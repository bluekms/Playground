using AuthLibrary.Extensions.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

[ApiController]
public sealed class FooController : ControllerBase
{
    public FooController()
    {
    }

    [HttpGet]
    [Route("Auth/Foo2")]
    [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = "ServiceApi")]
    public ActionResult<string> Foo([FromBody] Arguments args, CancellationToken cancellationToken)
    {
        return $"{args.Data}: Ok";
    }

    public sealed record Arguments(string Data);
}