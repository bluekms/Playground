using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CommonLibrary.Extensions;

public static class MySqlExtension
{
    public static void UseMySql<T>(this IServiceCollection services, string? connectionString)
        where T : DbContext
    {
        services.AddDbContextPool<T>(options =>
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