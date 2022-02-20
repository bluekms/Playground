using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AccountServer.Extensions.Authorizations;
using AccountServer.Handlers.Account;
using AccountServer.Handlers.Session;
using AccountServer.Models;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace AccountServer.Extensions.Authentication
{
    // https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
    public class SessionIdAuthenticationHandler : AuthenticationHandler<SessionIdAuthenticationSchemeOptions>
    {
        private readonly IQueryHandler<GetUserRoleQuery, string> getUserRole;
        private readonly IQueryHandler<GetAccountBySessionIdQuery, AccountData?> getAccount;
        private readonly ICommandHandler<InsertSessionIdCommand> insertSessionId;

        public SessionIdAuthenticationHandler(
            IOptionsMonitor<SessionIdAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IQueryHandler<GetUserRoleQuery, string> getUserRole,
            IQueryHandler<GetAccountBySessionIdQuery, AccountData?> getAccount,
            ICommandHandler<InsertSessionIdCommand> insertSessionId)
            : base(options, logger, encoder, clock)
        {
            this.getUserRole = getUserRole;
            this.getAccount = getAccount;
            this.insertSessionId = insertSessionId;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var sessionId = GetSessionIdFromHeaders();
            if (string.IsNullOrEmpty(sessionId))
            {
                return AuthenticateResult.NoResult();
            }

            var claimsIdentity = new ClaimsIdentity(SessionIdAuthenticationSchemeOptions.Name);
            claimsIdentity.AddClaim(CreateBuildConfigurationClaim());
            claimsIdentity.AddClaim(await CreateUserRoleClaim(sessionId));

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

            return AuthenticateResult.Success(authTicket);
        }

        private string? GetSessionIdFromHeaders()
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

            if (headerValue.Scheme != SessionIdAuthenticationSchemeOptions.Name)
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

        private async Task<Claim> CreateUserRoleClaim(string sessionId)
        {
            var userRole = await getUserRole.QueryAsync(new(sessionId));
            if (string.IsNullOrEmpty(userRole))
            {
                var account = await getAccount.QueryAsync(new(sessionId));
                if (account == null)
                {
                    throw new Exception("Invalid SessionId Token");
                }

                userRole = account.UserRole;
                await insertSessionId.ExecuteAsync(new(sessionId, userRole));
            }

            return new(ClientRoleRequirment.ClaimType, userRole);
        }
    }
}