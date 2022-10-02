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

    public DbSet<TargetTestRecord> TargetTestRecords { get; set; } = null!;

    //public DbSet<NameTestRecord> NameTestRecords { get; set; } = null!;

    //public DbSet<ArrayTestRecord> ArrayTestRecords { get; set; } = null!;

    //public DbSet<ClassListTestRecord> ClassListTestRecords { get; set; } = null!;

    //public DbSet<ComplexTestRecord> ComplexTestRecords { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}