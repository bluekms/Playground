using Microsoft.Extensions.DependencyInjection;

namespace AuthLibrary.Extensions.Authentication;

public static class OpenAuthenticationExtension
{
    public static void UseOpenAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(OpenAuthenticationSchemeOptions.Name)
            .AddScheme<OpenAuthenticationSchemeOptions, OpenAuthenticationHandler>(
                OpenAuthenticationSchemeOptions.Name, configureOptions => { });
    }
}
