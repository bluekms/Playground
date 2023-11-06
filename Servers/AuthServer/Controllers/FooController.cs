using System.Text.Json;
using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Extensions.Authorizations;
using AuthLibrary.Feature.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

[ApiController]
public sealed class FooController : ControllerBase
{
    [HttpPost]
    [Route("Auth/Foo")]
    [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = ApiPolicies.ServiceApi)]
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