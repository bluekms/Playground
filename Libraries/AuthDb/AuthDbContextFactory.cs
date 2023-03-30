using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace AuthDb;

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    private IConfigurationRoot config;

    public AuthDbContextFactory()
    {
        var path = Path.Join(AppContext.BaseDirectory, @"..\..\..\..\..\", @"Servers\AuthServer");
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (env != null)
        {
            if (env[0] != '.')
            {
                env = $".{env}";
            }
        }

        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile($"appsettings{env}.json", optional: false, reloadOnChange: true);

        config = builder.Build();
    }

    public AuthDbContext CreateDbContext(string[] args)
    {
        var conn = config.GetConnectionString(AuthDbContext.SectionName);

        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        optionsBuilder.UseMySql(
            conn,
            ServerVersion.Create(8, 0, 0, ServerType.MySql),
            builder =>
            {
                builder.EnableRetryOnFailure();
                builder.CommandTimeout(5);
            });

        return new AuthDbContext(optionsBuilder.Options);
    }
}