using Microsoft.EntityFrameworkCore;

namespace AuthDb;

public sealed class ReadOnlyAuthDbContext : IAuthDbContext
{
    private readonly AuthDbContext dbContext;

    public ReadOnlyAuthDbContext(AuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public DbSet<Foo> Foos => dbContext.Foos;
    public DbSet<Account> Accounts => dbContext.Accounts;
    public DbSet<Maintenance> Maintenance => dbContext.Maintenance;
    public DbSet<ServerRole> ServerRoles => dbContext.ServerRoles;
    public DbSet<Server> Servers => dbContext.Servers;
}
