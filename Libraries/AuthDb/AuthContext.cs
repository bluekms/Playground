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

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Credential> Credentials => Set<Credential>();
        public DbSet<Maintenance> Maintenance => Set<Maintenance>();
        public DbSet<Server> Servers => Set<Server>();
        public DbSet<Foo> Foos => Set<Foo>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Instructor> Instructors => Set<Instructor>();
        public DbSet<OfficeAssignment> OfficeAssignments => Set<OfficeAssignment>();
        public DbSet<CourseAssignment> CourseAssignments => Set<CourseAssignment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}