using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.DevRecords;
using StaticDataLibrary.DevRecords.TestRecords;

namespace StaticDataLibrary;

public class StaticDataContext : DbContext
{
    public StaticDataContext(DbContextOptions<StaticDataContext> options, string dbFileName) 
        : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, dbFileName);
    }

    public string DbPath { get; } = null!;

    public DbSet<TypeTestRecord> TypeTestTable { get; set; } = null!;
    public DbSet<TargetTestRecord> TargetTestTable { get; set; } = null!;
    public DbSet<ClassListTestRecord> ClassListTestTable { get; set; } = null!;
    public DbSet<ForeignTestRecord> ForeignTestTable { get; set; } = null!;
    public DbSet<MultiForeignTestRecord> MultiForeignTestTable { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}