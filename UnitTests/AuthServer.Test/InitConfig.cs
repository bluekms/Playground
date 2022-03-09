using Microsoft.Extensions.Configuration;

namespace AuthServer.Test
{
    public class InitConfig
    {
        public static IConfiguration Use()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.Testing.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}