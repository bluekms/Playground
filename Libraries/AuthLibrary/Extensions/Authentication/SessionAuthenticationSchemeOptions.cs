using Microsoft.AspNetCore.Authentication;

namespace AuthLibrary.Extensions.Authentication;

public class SessionAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public const string Name = "Session";
}
