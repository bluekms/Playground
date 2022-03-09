using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AuthServer.Extensions.Authorizations;
using AuthServer.Handlers.Account;
using AuthServer.Handlers.Session;
using AuthServer.Models;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace AuthServer.Extensions.Authentication
{
    public class SessionAuthenticationHandler : AuthenticationHandler<SessionAuthenticationSchemeOptions>
    {
        private readonly IQueryHandler<GetUserRoleQuery, UserRoles> getUserRole;
        private readonly IQueryHandler<GetAccountBySessionQuery, AccountData?> getAccount;
        private readonly ICommandHandler<AddSessionCommand> insertSessionId;

        public SessionAuthenticationHandler(
            IOptionsMonitor<SessionAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IQueryHandler<GetUserRoleQuery, UserRoles> getUserRole,
            IQueryHandler<GetAccountBySessionQuery, AccountData?> getAccount,
            ICommandHandler<AddSessionCommand> insertSessionId)
            : base(options, logger, encoder, clock)
        {
            this.getUserRole = getUserRole;
            this.getAccount = getAccount;
            this.insertSessionId = insertSessionId;
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
            var autorization = Request.Headers[HeaderNames.Authorization];
            if (string.IsNullOrEmpty(autorization))
            {
                return string.Empty;
            }

            if (!AuthenticationHeaderValue.TryParse(autorization, out var headerValue))
            {
                return string.Empty;
            }

            if (headerValue.Scheme != SessionAuthenticationSchemeOptions.Name)
            {
                return string.Empty;
            }

            return headerValue.Parameter;
        }

        private Claim CreateBuildConfigurationClaim()
        {
#if DEBUG
            var buildConfiguration = BuildConfigurationRequirment.BuildConfigurations.Debug;
#else
            var buildConfiguration = BuildConfigurationRequirement.BuildConfigurations.Release;
#endif

            return new(BuildConfigurationRequirment.ClaimType, buildConfiguration.ToString());
        }

        private async Task<Claim> CreateClientRoleClaim(string token)
        {
            var userRole = await getUserRole.QueryAsync(new(token));

            // TODO 추후 없으면 mysql에서 가져오는 로직 추가

            return new(UserRoleRequirment.ClaimType, userRole.ToString());
        }
    }
}