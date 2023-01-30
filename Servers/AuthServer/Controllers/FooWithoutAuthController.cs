using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Feature.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

public class FooWithoutAuthController : ControllerBase
{
    public FooWithoutAuthController()
    {
    }

    [HttpGet]
    [Route("Auth/Foo2")]
    [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = "ServiceApi")]
    public ActionResult<string> Foo([FromBody] Arguments args, SessionData session, CancellationToken cancellationToken)
    {
        return $"{args.Data}: Ok";
    }

    public sealed record Arguments(string Data);
}