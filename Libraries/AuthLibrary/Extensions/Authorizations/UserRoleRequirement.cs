using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthLibrary.Extensions.Authorizations;

public class UserRoleRequirement : IAuthorizationRequirement
{
    public const string ClaimType = "UserRole";

    public UserRoleRequirement(AccountRoles[] userRoles)
    {
        UserRoleList = new(userRoles.Select(x => x.ToString()));
    }

    public List<string> UserRoleList { get; }
}