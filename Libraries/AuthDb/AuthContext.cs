using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace AuthDb
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

        [AllowNull]
        public DbSet<Account> Accounts { get; set; }
        
        [AllowNull]
        public DbSet<Credential> Credentials { get; set; }
        
        [AllowNull]
        public DbSet<Maintenance> Maintenance { get; set; }
        
        [AllowNull]
        public DbSet<Server> Servers { get; set; }
        
        [AllowNull]
        public DbSet<Foo> Foos { get; set; }
        
        [AllowNull]
        public DbSet<Course> Courses { get; set; }
        
        [AllowNull]
        public DbSet<Enrollment> Enrollments { get; set; }
        
        [AllowNull]
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}