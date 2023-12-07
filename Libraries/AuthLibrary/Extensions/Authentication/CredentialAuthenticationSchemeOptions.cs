using Microsoft.AspNetCore.Authentication;

namespace AuthLibrary.Extensions.Authentication;

public class CredentialAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public const string Name = "Credential";
}
