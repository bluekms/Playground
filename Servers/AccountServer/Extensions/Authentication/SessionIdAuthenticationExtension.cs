using Microsoft.Extensions.DependencyInjection;

namespace AccountServer.Extensions.Authentication
{
    public static class SessionIdAuthenticationExtension
    {
        public static void UseSessionIdAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthentication(SessionIdAuthenticationSchemeOptions.Name)
                .AddScheme<SessionIdAuthenticationSchemeOptions, SessionIdAuthenticationHandler>(
                    SessionIdAuthenticationSchemeOptions.Name, configureOptions => { });
        }
    }
}