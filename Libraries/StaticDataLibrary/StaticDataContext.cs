using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.Records;

namespace StaticDataLibrary;

public class StaticDataContext : DbContext
{
    private const string DbFileName = "StaticData.db";

    public StaticDataContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, DbFileName);
    }

    public string DbPath { get; } = null!;

    //public DbSet<TargetTestRecord> TargetTestTable { get; set; } = null!;

    //public DbSet<NameTestRecord> NameTestTable { get; set; } = null!;

    public DbSet<ArrayTestRecord> ArrayTestTable { get; set; } = null!;

    //public DbSet<ClassListTestRecord> ClassListTestTable { get; set; } = null!;

    //public DbSet<ComplexTestRecord> ComplexTestTable { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}