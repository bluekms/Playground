using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace CommonLibrary.Extensions;

public static class PostgreSqlExtension
{
    private const string MigrationsHistoryTableName = "__EFMigrationsHistory";
    private const string MigrationsHistoryTableSchema = "main";

    public static void UsePostgreSql<T>(
        this IServiceCollection services,
        string? connectionString,
        string applicationName,
        bool isProduction)
        where T : DbContext
    {
        var conn = new NpgsqlConnectionStringBuilder(connectionString)
        {
            ApplicationName = applicationName,
        };

        if (!isProduction)
        {
            conn.LogParameters = true;
            conn.IncludeErrorDetail = true;
        }

        services.AddDbContext<T>(options =>
        {
            options.UseNpgsql(conn.ToString(), builder =>
            {
                builder.EnableRetryOnFailure();
                builder.MigrationsHistoryTable(MigrationsHistoryTableName, MigrationsHistoryTableSchema);
            });
        });
    }
}