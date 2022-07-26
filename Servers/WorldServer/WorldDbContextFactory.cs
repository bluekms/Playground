using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace WorldServer;

public class WorldDbContextFactory : IDesignTimeDbContextFactory<WorldDbContext>
{
    private readonly IConfigurationRoot config;

    public WorldDbContextFactory()
    {
        var path = Path.Join(AppContext.BaseDirectory, @"..\..\..\..\..\", @"Servers\WorldServer");
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

    public WorldDbContext CreateDbContext(string[] args)
    {
        var conn = config.GetConnectionString("WorldDb");

        var optionsBuilder = new DbContextOptionsBuilder<WorldDbContext>();
        optionsBuilder.UseMySql(
            conn,
            ServerVersion.Create(8, 0, 0, ServerType.MySql),
            builder =>
            {
                builder.EnableRetryOnFailure();
                builder.CommandTimeout(5);
            });

        return new WorldDbContext(optionsBuilder.Options);
    }
}