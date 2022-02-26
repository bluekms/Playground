using Microsoft.AspNetCore.Authentication;

namespace AuthServer.Extensions.Authentication
{
    public class CredentialAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string Name = "Credential";
    }
}