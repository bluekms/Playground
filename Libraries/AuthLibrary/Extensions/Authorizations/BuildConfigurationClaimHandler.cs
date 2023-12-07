using Microsoft.AspNetCore.Authorization;

namespace AuthLibrary.Extensions.Authorizations;

public class BuildConfigurationClaimHandler : AuthorizationHandler<BuildConfigurationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BuildConfigurationRequirement requirement)
    {
        if (context.User?.Identity == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (!context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var claim = context.User.Claims
            .FirstOrDefault(x => x.Type == BuildConfigurationRequirement.ClaimType);

        if (claim == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (claim.Value != requirement.BuildConfiguration.ToString())
        {
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
