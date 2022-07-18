using System.Linq;
using System.Threading.Tasks;
using AuthServer.Handlers.Maintenance;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthServer.Extensions.Authorizations
{
    public class UserRoleHandler : AuthorizationHandler<UserRoleRequirement>
    {
        private readonly IQueryHandler<IsMaintenanceTimeQuery, bool> isMaintenanceTime;

        public UserRoleHandler(IQueryHandler<IsMaintenanceTimeQuery, bool> isMaintenanceTime)
        {
            this.isMaintenanceTime = isMaintenanceTime;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRoleRequirement requirement)
        {
            if (context.User?.Identity == null)
            {
                context.Fail();
                return;
            }

            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var claim = context.User.Claims
                .FirstOrDefault(x => x.Type == UserRoleRequirement.ClaimType);

            if (claim == null)
            {
                context.Fail();
                return;
            }

            if (requirement.UserRoleList.All(x => x != claim.Value))
            {
                context.Fail();
                return;
            }

            if (claim.Value == UserRoles.User.ToString())
            {
                if (await isMaintenanceTime.QueryAsync(new()))
                {
                    context.Fail();
                    return;
                }
            }

            context.Succeed(requirement);
        }
    }
}