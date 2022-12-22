using Microsoft.AspNetCore.Authentication;

namespace AuthLibrary.Extensions.Authentication;

public class OpenAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public const string Name = "Open";
}