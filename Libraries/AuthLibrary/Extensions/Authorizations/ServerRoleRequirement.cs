using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthLibrary.Extensions.Authorizations;

public class ServerRoleRequirement : IAuthorizationRequirement
{
    public const string ClaimType = "ServerRole";

    public ServerRoleRequirement(ServerRoles serverRole)
    {
        ServerRole = serverRole;
    }

    public ServerRoles ServerRole { get; }
}
