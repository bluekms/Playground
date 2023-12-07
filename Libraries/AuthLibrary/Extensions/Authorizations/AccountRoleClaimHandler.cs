using AuthLibrary.Handlers;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthLibrary.Extensions.Authorizations;

public sealed class AccountRoleClaimHandler : AuthorizationHandler<AccountRoleRequirement>
{
    private readonly IQueryHandler<IsMaintenanceTimeQuery, bool> isMaintenanceTime;

    public AccountRoleClaimHandler(IQueryHandler<IsMaintenanceTimeQuery, bool> isMaintenanceTime)
    {
        this.isMaintenanceTime = isMaintenanceTime;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccountRoleRequirement requirement)
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
            .FirstOrDefault(x => x.Type == AccountRoleRequirement.ClaimType);

        if (claim == null)
        {
            context.Fail();
            return;
        }

        if (requirement.AccountRoleList.All(x => x != claim.Value))
        {
            context.Fail();
            return;
        }

        if (claim.Value == ResSignUp.Types.AccountRoles.User.ToString())
        {
            if (await isMaintenanceTime.QueryAsync(new(), CancellationToken.None))
            {
                context.Fail();
                return;
            }
        }

        context.Succeed(requirement);
    }
}
