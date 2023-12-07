using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AuthLibrary.Extensions.Authorizations;

/// <summary>
/// 세션 인증을 통해 사용할수 있는 Api들은 다음 정책을 따른다
/// AdminApi : OperationServer에서 AccountRoles Developer ~ WhitelistUser 까지 생성한다
/// ServiceApi : 대부분의 Api
/// OperationApi : 운영 및 치트관련 Api. Operation서버 이외에 World서버에서도 쓰일수 있다
/// </summary>
public static class ApiPolicies
{
    public const string AdminApi = "AdminApi";
    public const string ServiceApi = "ServiceApi";
    public const string OperationApi = "OperationApi";
}

public static class PermissionAuthorizationExtension
{
    public static void UsePermissionAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(ApiPolicies.AdminApi, policy =>
            {
                policy.Requirements.Add(new AccountRoleRequirement(new[]
                {
                    ResSignUp.Types.AccountRoles.Administrator,
                }));
            });

            options.AddPolicy(ApiPolicies.ServiceApi, policy =>
            {
                policy.Requirements.Add(new AccountRoleRequirement(new[]
                {
                    ResSignUp.Types.AccountRoles.Developer,
                    ResSignUp.Types.AccountRoles.AuthorizedUser,
                    ResSignUp.Types.AccountRoles.User,
                }));
            });

            options.AddPolicy(ApiPolicies.OperationApi, policy =>
            {
                policy.Requirements.Add(new BuildConfigurationRequirement(BuildConfigurationRequirement.BuildConfigurations.Debug));
                policy.Requirements.Add(new AccountRoleRequirement(new[]
                {
                    ResSignUp.Types.AccountRoles.Developer,
                    ResSignUp.Types.AccountRoles.OpUser,
                }));
            });
        });

        services.AddScoped<IAuthorizationHandler, BuildConfigurationClaimHandler>();
        services.AddScoped<IAuthorizationHandler, AccountRoleClaimHandler>();
    }
}
