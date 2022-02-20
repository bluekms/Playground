using Microsoft.AspNetCore.Authentication;

namespace AccountServer.Extensions.Authentication
{
    public class SessionIdAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string Name = "SessionId";
    }
}