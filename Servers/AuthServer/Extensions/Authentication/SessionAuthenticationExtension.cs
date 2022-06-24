using Microsoft.Extensions.DependencyInjection;
using Utf8Json.Resolvers;

namespace AuthServer.Extensions.Authentication
{
    public static class SessionAuthenticationExtension
    {
        public static void UseSessionAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthentication(SessionAuthenticationSchemeOptions.Name)
                .AddScheme<SessionAuthenticationSchemeOptions, SessionAuthenticationHandler>(
                    SessionAuthenticationSchemeOptions.Name, configureOptions => { });

            services.AddMvc(option =>
            {
                option.OutputFormatters.Clear();
                option.OutputFormatters.Add(new Utf8JsonOutputFormatter(StandardResolver.Default));
                option.InputFormatters.Clear();
                option.InputFormatters.Add(new Utf8JsonInputFormatter());
            });
        }
    }
}