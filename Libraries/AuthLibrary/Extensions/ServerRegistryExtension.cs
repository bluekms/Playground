using System.Net;
using CommonLibrary.Models;
using CommonLibrary.ServerRegistry;
using CommonLibrary.Worker;
using Polly;
using Polly.Extensions.Http;

public class ServerRegistryExtension
{
    public static IServiceCollection UseServerRegistry(this IServiceCollection services, IConfigurationSection section)
    {
        // https://docs.microsoft.com/ko-kr/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
        // 2초에서 시작하여 지수 다시 시도를 6회 시도
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        services.AddOptions<ServerRegistryOptions>().Bind(section);
        services.AddHttpClient<ServerRegistryClient>().AddPolicyHandler(retryPolicy);
        services.UseWorkService<ServerRegister, ServerRegisterOptionProvider>();
        return services;
    }
}