using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace AccountServer.Extensions.Authorizations
{
    public class ServerRoleRequirment : IAuthorizationRequirement
    {
        public const string ClaimType = "ServerRole";

        public ServerRoleRequirment(ServerRoles serverRole)
        {
            ServerRole = serverRole;
        }

        public ServerRoles ServerRole { get; }
    }
}