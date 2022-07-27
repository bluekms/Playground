using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AuthLibrary.Extensions;

public static class RedisCacheExtension
{
    public static void UseRedisCache(this IServiceCollection services, string? connectionString)
    {
        Console.WriteLine($"kkk : {connectionString ?? "null"}");

        var redisConnection = ConnectionMultiplexer.Connect(connectionString);

        services.AddScoped(_ => redisConnection.GetDatabase());
    }
}