using System.Collections.Generic;
using System.Linq;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace AccountServer.Extensions.Authorizations
{
    public class UserRoleRequirment : IAuthorizationRequirement
    {
        public const string ClaimType = "UserRole";

        public UserRoleRequirment(UserRoles[] userRoles)
        {
            UserRoleList = new(userRoles.Select(x => x.ToString()));
        }

        public List<string> UserRoleList { get; }
    }
}