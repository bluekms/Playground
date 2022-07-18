using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AuthServer.Test
{
    public class InitConfig
    {
        public static IConfiguration Use()
        {
            var path = Path.Join(AppContext.BaseDirectory, @"../../..");

            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile($"appsettings.Test.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}