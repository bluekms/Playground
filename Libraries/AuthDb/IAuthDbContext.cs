using Microsoft.EntityFrameworkCore;

namespace AuthDb;

public interface IAuthDbContext
{
    public DbSet<Foo> Foos { get; }
    public DbSet<Account> Accounts { get; }
    public DbSet<Password> Passwords { get; }
    public DbSet<Maintenance> Maintenances { get; }
    public DbSet<ServerRole> ServerRoles { get; }
    public DbSet<Server> Servers { get; }
}
