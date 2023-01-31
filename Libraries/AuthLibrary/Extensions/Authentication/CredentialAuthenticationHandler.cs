using System.Security.Claims;
using System.Text.Encodings.Web;
using AuthLibrary.Extensions.Authorizations;
using AuthLibrary.Handlers;
using AuthLibrary.Utility;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthLibrary.Extensions.Authentication;

public sealed class CredentialAuthenticationHandler : AuthenticationHandler<CredentialAuthenticationSchemeOptions>
{
    private const string AuthType = "Bearer";

    private readonly IQueryHandler<GetServerRoleQuery, ServerRoles> getServerRole;

    public CredentialAuthenticationHandler(
        IOptionsMonitor<CredentialAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IQueryHandler<GetServerRoleQuery, ServerRoles> getServerRole)
        : base(options, logger, encoder, clock)
    {
        this.getServerRole = getServerRole;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = BearerTokenParser.GetBearerToken(Request);

        var claimsIdentity = new ClaimsIdentity(CredentialAuthenticationSchemeOptions.Name);
        claimsIdentity.AddClaim(await CreateServerRoleClaim(token));

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

        return AuthenticateResult.Success(authTicket);
    }

    private async Task<Claim> CreateServerRoleClaim(string token)
    {
        var serverRole = await getServerRole.QueryAsync(new(token), CancellationToken.None);

        return new(ServerRoleRequirement.ClaimType, serverRole.ToString());
    }
}