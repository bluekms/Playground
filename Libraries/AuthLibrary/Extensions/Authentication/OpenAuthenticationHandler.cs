using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthLibrary.Extensions.Authentication;

public class OpenAuthenticationHandler : AuthenticationHandler<OpenAuthenticationSchemeOptions>
{
    public OpenAuthenticationHandler(
        IOptionsMonitor<OpenAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claimsIdentity = new ClaimsIdentity(OpenAuthenticationSchemeOptions.Name);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        var result = AuthenticateResult.Success(authTicket);
        return await Task.FromResult(result);
    }
}
