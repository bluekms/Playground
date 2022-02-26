using AuthDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthServer.Extensions
{
    public static class AuthDbExtension
    {
        public static void UseMySql(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AuthContext>(options =>
            {
                options.UseMySQL(connectionString);
            });
        }
    }
}