using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AuthLibrary.Extensions;

public static class RedisCacheExtension
{
    public const string SectionName = "RedisCache";

    public static void UseRedisCache(this IServiceCollection services, string connectionString)
    {
        var redisConnection = ConnectionMultiplexer.Connect(connectionString);
        services.AddSingleton<IConnectionMultiplexer>(_ => redisConnection);
    }
}