using System.Security.Claims;
using System.Text.Encodings.Web;
using AuthLibrary.Extensions.Authorizations;
using AuthLibrary.Feature.Session;
using AuthLibrary.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthLibrary.Extensions.Authentication;

public class SessionAuthenticationHandler : AuthenticationHandler<SessionAuthenticationSchemeOptions>
{
    private readonly SessionStore sessionStore;

    public SessionAuthenticationHandler(
        IOptionsMonitor<SessionAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        SessionStore sessionStore)
        : base(options, logger, encoder, clock)
    {
        this.sessionStore = sessionStore;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = BearerTokenParser.GetBearerToken(Request);
        var session = await sessionStore.GetAsync(token, CancellationToken.None);
        Context.Features.Set(session);

        var claimsIdentity = new ClaimsIdentity(SessionAuthenticationSchemeOptions.Name);
        claimsIdentity.AddClaim(CreateBuildConfigurationClaim());
        claimsIdentity.AddClaim(CreateAccountRoleClaim(session));

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

        return AuthenticateResult.Success(authTicket);
    }

    private static Claim CreateBuildConfigurationClaim()
    {
#if DEBUG
        var buildConfiguration = BuildConfigurationRequirement.BuildConfigurations.Debug;
#else
        var buildConfiguration = BuildConfigurationRequirement.BuildConfigurations.Release;
#endif

        return new(BuildConfigurationRequirement.ClaimType, buildConfiguration.ToString());
    }

    private static Claim CreateAccountRoleClaim(SessionInfo session)
    {
        return new(AccountRoleRequirement.ClaimType, session.AccountRole.ToString());
    }
}
