using Microsoft.Extensions.Options;

namespace WorldServer.Extensions;

// https://docs.microsoft.com/ko-kr/aspnet/core/grpc/configuration?view=aspnetcore-6.0
// For macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
public static class GrpcExtension
{
    public static void UseGrpc(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
#if Debug
            options.EnableDetailedErrors = true;
#endif
            options.MaxReceiveMessageSize = 2 * 1024 * 1024;    // 2MB
            options.MaxSendMessageSize = 5 * 1024 * 1024;       // 5MB
        });
    }
}