using AuthLibrary.Extensions.Authentication;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary;
using StaticDataLibrary.DevDataObjects;
using StaticDataLibrary.DevRecords;
using WorldServer.Handlers.Foo;

namespace WorldServer.Controllers.Foo;

[ApiController]
public sealed class AddFooController : ControllerBase
{
    private readonly ICommandHandler<AddFooCommand> addFoo;
    private readonly StaticDataContext staticData;

    public AddFooController(
        ICommandHandler<AddFooCommand> addFoo,
        StaticDataContext staticData)
    {
        this.addFoo = addFoo;
        this.staticData = staticData;
    }

    [HttpPost]
    [Route("World/Foo/Add")]
    [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = "ServiceApi")]
    public async Task<ActionResult<string>> HandleAsync([FromBody] ArgumentData args)
    {
        await addFoo.ExecuteAsync(new(args.Data));

        var id = int.Parse(args.Data);

        var record = await staticData.ArrayTestTable.SingleAsync(x => x.Id == id);

        // 기본 생성자 방식
        var data = new ArrayTest(record);

        return $"Ok: {data}";
    }

    public sealed record ArgumentData(string Data);
}