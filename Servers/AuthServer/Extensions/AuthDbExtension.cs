using AuthDb;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace AuthServer.Extensions
{
    public static class AuthDbExtension
    {
        public static void UseMySql(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContextPool<AuthContext>(options =>
            {
                options.UseMySql(
                    connectionString,
                    ServerVersion.Create(8, 0, 0, ServerType.MySql));
            });
        }
    }
}