using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

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
        var conn = config.GetConnectionString(AuthDbContext.Name);

        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        optionsBuilder.UseNpgsql(conn, x => x.MigrationsHistoryTable("__EFMigrationsHistory", AuthDbContext.Name));

        return new AuthDbContext(optionsBuilder.Options);
    }
}
