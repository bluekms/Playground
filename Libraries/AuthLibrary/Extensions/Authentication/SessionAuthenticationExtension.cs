using Microsoft.Extensions.DependencyInjection;

namespace AuthLibrary.Extensions.Authentication;

public static class SessionAuthenticationExtension
{
    public static void UseSessionAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(SessionAuthenticationSchemeOptions.Name)
            .AddScheme<SessionAuthenticationSchemeOptions, SessionAuthenticationHandler>(
                SessionAuthenticationSchemeOptions.Name, configureOptions => { });
    }
}