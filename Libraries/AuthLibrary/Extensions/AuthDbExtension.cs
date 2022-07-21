using AuthDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace AuthLibrary.Extensions;

public static class AuthDbExtension
{
    public static void UseMySql(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContextPool<AuthDbContext>(options =>
        {
            options.UseMySql(
                connectionString,
                ServerVersion.Create(8, 0, 0, ServerType.MySql),
                builder =>
                {
                    builder.EnableRetryOnFailure();
                    builder.CommandTimeout(5);
                });
        });
    }
}