using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace CommonLibrary.Extensions;

public static class PostgreSqlExtension
{
    private const string MigrationsHistoryTableName = "__EFMigrationsHistory";

    public static void UsePostgreSql<TContext, TReadOnlyContext>(
        this IServiceCollection services,
        string? connectionString,
        string applicationName,
        string schemaName)
        where TContext : DbContext
        where TReadOnlyContext : class
    {
        var conn = new NpgsqlConnectionStringBuilder(connectionString)
        {
            ApplicationName = applicationName,
        };

        var serviceProvider = services.BuildServiceProvider();
        var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
        if (!environment.IsProduction())
        {
            conn.LogParameters = true;
            conn.IncludeErrorDetail = true;
        }

        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(conn.ToString(), builder =>
            {
                builder.EnableRetryOnFailure();
                builder.MigrationsHistoryTable(MigrationsHistoryTableName, schemaName);
            });
        });

        services.AddScoped<TReadOnlyContext>(provider =>
        {
            var dbContext = provider.GetRequiredService<TContext>();
            return (TReadOnlyContext)Activator.CreateInstance(typeof(TReadOnlyContext), dbContext)!;
        });
    }
}
