using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using AuthLibrary.Extensions.Authorizations;
using AuthLibrary.Handlers;
using AuthLibrary.Models;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace AuthLibrary.Extensions.Authentication;

public class SessionAuthenticationHandler : AuthenticationHandler<SessionAuthenticationSchemeOptions>
{
    private const string AuthType = "Bearer";

    private readonly IQueryHandler<GetUserRoleQuery, UserRoles?> getUserRole;
    private readonly IQueryHandler<GetAccountBySessionQuery, AccountData?> getAccount;
    private readonly ICommandHandler<AddSessionCommand> addSessionId;

    public SessionAuthenticationHandler(
        IOptionsMonitor<SessionAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IQueryHandler<GetUserRoleQuery, UserRoles?> getUserRole,
        IQueryHandler<GetAccountBySessionQuery, AccountData?> getAccount,
        ICommandHandler<AddSessionCommand> addSessionId)
        : base(options, logger, encoder, clock)
    {
        this.getUserRole = getUserRole;
        this.getAccount = getAccount;
        this.addSessionId = addSessionId;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = GetSessionToken();
        if (string.IsNullOrEmpty(token))
        {
            return AuthenticateResult.NoResult();
        }

        var claimsIdentity = new ClaimsIdentity(SessionAuthenticationSchemeOptions.Name);
        claimsIdentity.AddClaim(CreateBuildConfigurationClaim());
        claimsIdentity.AddClaim(await CreateClientRoleClaim(token));

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

        return AuthenticateResult.Success(authTicket);
    }

    private string? GetSessionToken()
    {
        var authorization = Request.Headers[HeaderNames.Authorization];
        if (string.IsNullOrEmpty(authorization))
        {
            return string.Empty;
        }

        if (!AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
        {
            return string.Empty;
        }

        if (headerValue.Scheme != AuthType)
        {
            return string.Empty;
        }

        return headerValue.Parameter;
    }

    private Claim CreateBuildConfigurationClaim()
    {
#if DEBUG
        var buildConfiguration = BuildConfigurationRequirement.BuildConfigurations.Debug;
#else
        var buildConfiguration = BuildConfigurationRequirement.BuildConfigurations.Release;
#endif

        return new(BuildConfigurationRequirement.ClaimType, buildConfiguration.ToString());
    }

    private async Task<Claim> CreateClientRoleClaim(string token)
    {
        var userRole = await getUserRole.QueryAsync(new(token));

        if (userRole is null)
        {
            var accountData = await getAccount.QueryAsync(new(token));
            if (accountData == null)
            {
                throw new KeyNotFoundException();
            }

            await addSessionId.ExecuteAsync(new(accountData.Token, accountData.Role));

            userRole = accountData.Role;
        }

        return new(UserRoleRequirement.ClaimType, userRole.ToString()!);
    }
}