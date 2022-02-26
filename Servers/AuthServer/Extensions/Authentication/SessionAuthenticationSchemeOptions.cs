using Microsoft.AspNetCore.Authentication;

namespace AuthServer.Extensions.Authentication
{
    public class SessionAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string Name = "Session";
    }
}