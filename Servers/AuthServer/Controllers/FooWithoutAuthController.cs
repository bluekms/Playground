using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

public class FooWithoutAuthController : ControllerBase
{
    [HttpPost]
    [Route("Auth/Foo2")]
    public ActionResult<string> Foo(
        [FromBody] Arguments args,
        CancellationToken cancellationToken)
    {
        // TODO Open일때 Session을 인자로 받지 못하도록 유닛테스트 걸자
        return $"{args.Data}: Ok.";
    }

    public sealed record Arguments(string Data);
}