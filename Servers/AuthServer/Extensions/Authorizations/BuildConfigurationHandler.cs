using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AuthServer.Extensions.Authorizations
{
    public class BuildConfigurationHandler : AuthorizationHandler<BuildConfigurationRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BuildConfigurationRequirment requirment)
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
                .FirstOrDefault(x => x.Type == BuildConfigurationRequirment.ClaimType);

            if (claim == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (claim.Value != requirment.BuildConfiguration.ToString())
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirment);
            return Task.CompletedTask;
        }
    }
}