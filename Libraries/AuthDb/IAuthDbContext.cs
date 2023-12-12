using Microsoft.EntityFrameworkCore;

namespace AuthDb;

public interface IAuthDbContext
{
    public DbSet<Foo> Foos { get; }
    public DbSet<Account> Accounts { get; }
    public DbSet<Maintenance> Maintenance { get; }
    public DbSet<ServerRole> ServerRoles { get; }
    public DbSet<Server> Servers { get; }
}
