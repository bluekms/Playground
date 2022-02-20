using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AccountServer.Extensions.Authorizations
{
    public static class PermissionAuthorizationExtension
    {
        public static void UsePermissionAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ServiceApi", policy =>
                {
                    policy.Requirements.Add(new ClientRoleRequirment(new[]
                    {
                        ClientRoleRequirment.ClientRoles.Developer,
                        ClientRoleRequirment.ClientRoles.WhitelistedUser,
                        ClientRoleRequirment.ClientRoles.User,
                    }));
                });

                options.AddPolicy("CheatApi", policy =>
                {
                    policy.Requirements.Add(new BuildConfigurationRequirment(BuildConfigurationRequirment.BuildConfigurations.Debug));
                    policy.Requirements.Add(new ClientRoleRequirment(new[]
                    {
                        ClientRoleRequirment.ClientRoles.Developer,
                    }));
                });

                options.AddPolicy("InternalApi", policy =>
                {
                    policy.Requirements.Add(new ClientRoleRequirment(new[]
                    {
                        ClientRoleRequirment.ClientRoles.Administrator,
                        ClientRoleRequirment.ClientRoles.Developer,
                        ClientRoleRequirment.ClientRoles.InternalService,
                    }));
                });
            });

            services.AddScoped<IAuthorizationHandler, BuildConfigurationHandler>();
            services.AddScoped<IAuthorizationHandler, ClientRoleHandler>();
        }
    }
}