using AuthLibrary.Extensions.Authentication;
using CommonLibrary.Handlers;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using World;
using WorldServer.Handlers.Foo;

namespace WorldServer.Services;

public class FooService : Foo.FooBase
{
    private readonly ICommandHandler<AddFooCommand> addFoo;
    private readonly IQueryHandler<GetFooQuery, List<string>> getFoo;

    public FooService(
        ICommandHandler<AddFooCommand> addFoo,
        IQueryHandler<GetFooQuery, List<string>> getFoo)
    {
        this.addFoo = addFoo;
        this.getFoo = getFoo;
    }

    [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = "ServiceApi")]
    public override async Task<AddFooResult> AddFooHandler(AddFooRequest request, ServerCallContext context)
    {
        await addFoo.ExecuteAsync(new(request.Data));

        return new();
    }

    [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = "ServiceApi")]
    public override async Task<GetFooResult> GetFooHandler(GetFooRequest request, ServerCallContext context)
    {
        var list = await getFoo.QueryAsync(new(request.Seq));

        return new()
        {
            Data = { list },
        };
    }
}