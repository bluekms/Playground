using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AuthLibrary.Extensions;

public static class RedisCacheExtension
{
    public static void UseRedisCache(this IServiceCollection services, string? connectionString)
    {
        Console.WriteLine("Start");
        Console.WriteLine($"kkk : {connectionString ?? "null"}");
        Console.WriteLine($"appsettings.Docker.json : {File.Exists("appsettings.Docker.json")}");
        Console.WriteLine(File.ReadAllText("appsettings.Docker.json"));
        Console.WriteLine("End");

        var redisConnection = ConnectionMultiplexer.Connect(connectionString);

        services.AddScoped(_ => redisConnection.GetDatabase());
    }
}