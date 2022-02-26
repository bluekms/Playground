using System.Linq;
using System.Threading.Tasks;
using AccountServer.Handlers.Maintenance;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace AccountServer.Extensions.Authorizations
{
    public class UserRoleHandler : AuthorizationHandler<UserRoleRequirment>
    {
        private readonly IQueryHandler<IsMaintenanceTimeQuery, bool> isMaintenanceTime;

        public UserRoleHandler(IQueryHandler<IsMaintenanceTimeQuery, bool> isMaintenanceTime)
        {
            this.isMaintenanceTime = isMaintenanceTime;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRoleRequirment requirement)
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
                .FirstOrDefault(x => x.Type == UserRoleRequirment.ClaimType);

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