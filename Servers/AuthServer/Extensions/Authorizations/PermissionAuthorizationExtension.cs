using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AuthServer.Extensions.Authorizations
{
    public static class PermissionAuthorizationExtension
    {
        public static void UsePermissionAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ServiceApi", policy =>
                {
                    policy.Requirements.Add(new UserRoleRequirment(new[]
                    {
                        UserRoles.Developer,
                        UserRoles.WhitelistedUser,
                        UserRoles.User,
                    }));
                });

                options.AddPolicy("CheatApi", policy =>
                {
                    policy.Requirements.Add(new BuildConfigurationRequirment(BuildConfigurationRequirment.BuildConfigurations.Debug));
                    policy.Requirements.Add(new UserRoleRequirment(new[]
                    {
                        UserRoles.Developer,
                    }));
                });

                options.AddPolicy("InternalApi", policy =>
                {
                    policy.Requirements.Add(new UserRoleRequirment(new[]
                    {
                        UserRoles.Administrator,
                        UserRoles.Developer,
                        UserRoles.InternalService,
                    }));
                });
            });

            services.AddScoped<IAuthorizationHandler, BuildConfigurationHandler>();
            services.AddScoped<IAuthorizationHandler, UserRoleHandler>();
        }
    }
}