using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AccountServer.Extensions.Authorizations
{
    public class FooHandler : AuthorizationHandler<FooRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FooRequirement requirement)
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

            var hasClaim = context.User.Claims.Any(x => x.Type == "FooType");
            if (hasClaim)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}