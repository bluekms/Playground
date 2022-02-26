using Microsoft.Extensions.DependencyInjection;

namespace AccountServer.Extensions.Authentication
{
    public static class CredentialAuthenticationExtension
    {
        public static void UseCredentialAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthentication(CredentialAuthenticationSchemeOptions.Name)
                .AddScheme<CredentialAuthenticationSchemeOptions, CredentialAuthenticationHandler>(
                    CredentialAuthenticationSchemeOptions.Name, configureOptions => { });
        }
    }
}