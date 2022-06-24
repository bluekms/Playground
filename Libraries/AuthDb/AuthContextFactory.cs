using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace AuthDb;

public class AuthContextFactory : IDesignTimeDbContextFactory<AuthContext>
{
    public AuthContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<AuthContext>();
        optionBuilder.UseMySql($"Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;", ServerVersion.Create(8, 0, 0, ServerType.MySql));

        return new AuthContext(optionBuilder.Options);
    }
}