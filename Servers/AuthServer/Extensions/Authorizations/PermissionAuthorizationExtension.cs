using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthServer.Extensions.Authorizations
{
    public static class PermissionAuthorizationExtension
    {
        public static void UsePermissionAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminApi", policy =>
                {
                    policy.Requirements.Add(new UserRoleRequirement(new[]
                    {
                        UserRoles.Administrator,
                    }));
                });

                options.AddPolicy("ServiceApi", policy =>
                {
                    policy.Requirements.Add(new UserRoleRequirement(new[]
                    {
                        UserRoles.Developer,
                        UserRoles.WhitelistUser,
                        UserRoles.User,
                    }));
                });

                options.AddPolicy("CheatApi", policy =>
                {
                    policy.Requirements.Add(new BuildConfigurationRequirement(BuildConfigurationRequirement.BuildConfigurations.Debug));
                    policy.Requirements.Add(new UserRoleRequirement(new[]
                    {
                        UserRoles.Developer,
                        UserRoles.OpUser,
                    }));
                });
            });

            services.AddScoped<IAuthorizationHandler, BuildConfigurationHandler>();
            services.AddScoped<IAuthorizationHandler, UserRoleHandler>();
        }
    }
}