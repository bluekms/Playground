using Microsoft.AspNetCore.Authentication;

namespace AccountServer.Extensions.Authentication
{
    public class CredentialAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string Name = "Credential";
    }
}