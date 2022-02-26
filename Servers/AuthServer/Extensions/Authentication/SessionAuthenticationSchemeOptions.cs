using Microsoft.AspNetCore.Authentication;

namespace AccountServer.Extensions.Authentication
{
    public class SessionAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string Name = "Session";
    }
}