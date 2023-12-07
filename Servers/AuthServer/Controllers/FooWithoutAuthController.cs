using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

public class FooWithoutAuthController : ControllerBase
{
    [HttpPost]
    [Route("Auth/Foo2")]
    public ActionResult<string> Foo(
        [FromBody] ReqFoo request,
        CancellationToken cancellationToken)
    {
        throw new Exception($"{request.Data}");
    }
}
