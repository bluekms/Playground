using System.Collections.Generic;
using System.Linq;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthServer.Extensions.Authorizations
{
    public class UserRoleRequirement : IAuthorizationRequirement
    {
        public const string ClaimType = "UserRole";

        public UserRoleRequirement(UserRoles[] userRoles)
        {
            UserRoleList = new(userRoles.Select(x => x.ToString()));
        }

        public List<string> UserRoleList { get; }
    }
}