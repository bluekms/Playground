using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AuthLibrary.Extensions.Authorizations;

public static class PermissionAuthorizationExtension
{
    public static void UsePermissionAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
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

        services.AddScoped<IAuthorizationHandler, BuildConfigurationClaimHandler>();
        services.AddScoped<IAuthorizationHandler, UserRoleClaimHandler>();
    }
}