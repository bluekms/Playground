using AuthDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthServer.Extensions
{
    public static class AuthDbExtension
    {
        public static void UseMySql(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<AuthContext>(options =>
            {
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect("connectionString"));
            });
        }
    }
}