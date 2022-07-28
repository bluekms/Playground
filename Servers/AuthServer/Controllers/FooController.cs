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

    [HttpPost]
    [Route("Auth/Foo")]
    [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = "ServiceApi")]
    public async Task<ActionResult<string>> Foo([FromBody] ArgumentData args)
    {
        try
        {
            return await HandleAsync(args);
        }
        catch (Exception e)
        {
            return NotFound($"{e.Message}:{e.InnerException?.Message ?? string.Empty}");
        }
    }

    private async Task<string> HandleAsync(ArgumentData args)
    {
        return await Task.Run(() => args.Data + ": OK");
    }

    public sealed record ArgumentData(string Data);
}