using System.Security.Claims;
using System.Text.Encodings.Web;
using AuthLibrary.Extensions.Authorizations;
using AuthLibrary.Handlers;
using AuthLibrary.Models;
using AuthLibrary.Utility;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthLibrary.Extensions.Authentication;

public class SessionAuthenticationHandler : AuthenticationHandler<SessionAuthenticationSchemeOptions>
{
    private readonly IQueryHandler<GetAccountRoleQuery, AccountRoles?> getUserRole;
    private readonly IQueryHandler<GetAccountBySessionQuery, AccountData?> getAccount;
    private readonly ICommandHandler<AddSessionCommand> addSessionId;

    public SessionAuthenticationHandler(
        IOptionsMonitor<SessionAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IQueryHandler<GetAccountRoleQuery, AccountRoles?> getUserRole,
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
        var token = BearerTokenParser.GetBearerToken(Request);

        var claimsIdentity = new ClaimsIdentity(SessionAuthenticationSchemeOptions.Name);
        claimsIdentity.AddClaim(CreateBuildConfigurationClaim());
        claimsIdentity.AddClaim(await CreateClientRoleClaim(token));

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

        return AuthenticateResult.Success(authTicket);
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
        var userRole = await getUserRole.QueryAsync(new(token), CancellationToken.None);

        if (userRole is null)
        {
            var accountData = await getAccount.QueryAsync(new(token), CancellationToken.None);
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