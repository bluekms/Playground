using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AuthLibrary.Extensions;

public static class RedisCacheExtension
{
    public static void UseRedisCache(this IServiceCollection services, string? connectionString)
    {
        Console.WriteLine("Start");
        Console.WriteLine($"kkk : {connectionString ?? "null"}");
        Console.WriteLine($"appsettings.json : {File.Exists("appsettings.json")}");
        Console.WriteLine("End");

        var redisConnection = ConnectionMultiplexer.Connect(connectionString);

        services.AddScoped(_ => redisConnection.GetDatabase());
    }
}