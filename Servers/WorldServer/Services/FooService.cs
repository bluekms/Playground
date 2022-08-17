using AuthLibrary.Extensions.Authentication;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using World;

namespace WorldServer.Services;

public class FooService : Foo.FooBase
{
    private readonly ILogger<FooService> logger;

    public FooService(ILogger<FooService> logger)
    {
        this.logger = logger;
    }

    [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = "ServiceApi")]
    public override Task<FooResult> RequestHandler(FooRequest request, ServerCallContext context)
    {
        logger.LogInformation($"Data: {request.Data}");
        return Task.FromResult(new FooResult()
        {
            Result = "Ok",
        });
    }
}