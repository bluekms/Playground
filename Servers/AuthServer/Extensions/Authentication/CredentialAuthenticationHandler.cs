using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AuthServer.Extensions.Authorizations;
using AuthServer.Handlers.Credential;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace AuthServer.Extensions.Authentication
{
    public class CredentialAuthenticationHandler : AuthenticationHandler<CredentialAuthenticationSchemeOptions>
    {
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
            var token = GetCredentialToken();
            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.NoResult();
            }

            var claimsIdentity = new ClaimsIdentity(CredentialAuthenticationSchemeOptions.Name);
            claimsIdentity.AddClaim(await CreateServerRoleClaim(token));

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

            return AuthenticateResult.Success(authTicket);
        }

        private string? GetCredentialToken()
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

            if (headerValue.Scheme != CredentialAuthenticationSchemeOptions.Name)
            {
                return string.Empty;
            }

            return headerValue.Parameter;
        }

        private async Task<Claim> CreateServerRoleClaim(string token)
        {
            var serverRole = await getServerRole.QueryAsync(new(token));

            return new(ServerRoleRequirment.ClaimType, serverRole.ToString());
        }
    }
}