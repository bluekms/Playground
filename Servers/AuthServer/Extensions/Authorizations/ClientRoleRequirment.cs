using System.Collections.Generic;
using System.Linq;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace AccountServer.Extensions.Authorizations
{
    public class ClientRoleRequirment : IAuthorizationRequirement
    {
        public const string ClaimType = "ClientRole";

        public ClientRoleRequirment(ClientRoles[] clientRoles)
        {
            ClientRoleList = new(clientRoles.Select(x => x.ToString()));
        }

        public List<string> ClientRoleList { get; }
    }
}