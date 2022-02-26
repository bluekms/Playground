using CommonLibrary.Models;
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
                        ClientRoles.Developer,
                        ClientRoles.WhitelistedUser,
                        ClientRoles.User,
                    }));
                });

                options.AddPolicy("CheatApi", policy =>
                {
                    policy.Requirements.Add(new BuildConfigurationRequirment(BuildConfigurationRequirment.BuildConfigurations.Debug));
                    policy.Requirements.Add(new ClientRoleRequirment(new[]
                    {
                        ClientRoles.Developer,
                    }));
                });

                options.AddPolicy("InternalApi", policy =>
                {
                    policy.Requirements.Add(new ClientRoleRequirment(new[]
                    {
                        ClientRoles.Administrator,
                        ClientRoles.Developer,
                        ClientRoles.InternalService,
                    }));
                });
            });

            services.AddScoped<IAuthorizationHandler, BuildConfigurationHandler>();
            services.AddScoped<IAuthorizationHandler, ClientRoleHandler>();
        }
    }
}