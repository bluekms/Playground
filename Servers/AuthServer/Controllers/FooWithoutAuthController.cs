using System.Text.Json;
using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Extensions.Authorizations;
using AuthLibrary.Feature.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

public class FooWithoutAuthController : ControllerBase
{
    [HttpPost]
    [Route("Auth/Foo2")]
    public ActionResult<string> Foo(
        SessionInfo session,
        [FromBody] Arguments args,
        CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(session);
        return $"{args.Data}: Ok. session: {json}";
    }

    public sealed record Arguments(string Data);
}