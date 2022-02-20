using System.Linq;
using System.Threading.Tasks;
using AccountServer.Handlers.Maintenance;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace AccountServer.Extensions.Authorizations
{
    public class ClientRoleHandler : AuthorizationHandler<ClientRoleRequirment>
    {
        private readonly IQueryHandler<IsMaintenanceTimeQuery, bool> isMaintenanceTime;

        public ClientRoleHandler(IQueryHandler<IsMaintenanceTimeQuery, bool> isMaintenanceTime)
        {
            this.isMaintenanceTime = isMaintenanceTime;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClientRoleRequirment requirement)
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
                .FirstOrDefault(x => x.Type == ClientRoleRequirment.ClaimType);

            if (claim == null)
            {
                context.Fail();
                return;
            }

            if (requirement.ClientRoleList.All(x => x != claim.Value))
            {
                context.Fail();
                return;
            }

            if (claim.Value == ClientRoleRequirment.ClientRoles.User.ToString())
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