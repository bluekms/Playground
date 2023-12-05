using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthLibrary.Extensions.Authorizations;

public class AccountRoleRequirement : IAuthorizationRequirement
{
    public const string ClaimType = "AccountRole";

    public AccountRoleRequirement(ResSignUp.Types.AccountRoles[] userRoles)
    {
        AccountRoleList = new(userRoles.Select(x => x.ToString()));
    }

    public List<string> AccountRoleList { get; }
}