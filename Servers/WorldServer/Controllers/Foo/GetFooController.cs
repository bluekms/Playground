using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Feature.Session;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldServer.Handlers.Foo;

namespace WorldServer.Controllers.Foo;

[ApiController]
public sealed class GetFooController : ControllerBase
{
    private readonly IQueryHandler<GetFooQuery, List<string>> getFoo;

    public GetFooController(IQueryHandler<GetFooQuery, List<string>> getFoo)
    {
        this.getFoo = getFoo;
    }

    [HttpPost]
    [Route("World/Foo/Get")]
    [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = "ServiceApi")]
    public async Task<ActionResult<List<string>>> HandleAsync([FromBody] ArgumentData args, SessionData session, CancellationToken cancellationToken)
    {
        return await getFoo.QueryAsync(new(args.Seq), cancellationToken);
    }

    public sealed record ArgumentData(long Seq);
}