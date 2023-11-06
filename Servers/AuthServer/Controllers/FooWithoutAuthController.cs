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
        // TODO Open일때 Session을 인자로 받지 못하도록 유닛테스트 걸자
        return $"{request.Data}: Ok.";
    }
}