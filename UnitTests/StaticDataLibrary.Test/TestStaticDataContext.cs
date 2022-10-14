using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.DevRecords;

namespace StaticDataLibrary.Test;

public class TestStaticDataContext : DbContext
{
    private const string DbFileName = "StaticData.db";

    protected TestStaticDataContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, DbFileName);
    }
    
    public string DbPath { get; } = null!;

    public DbSet<TargetTestRecord> TargetTestTable { get; set; } = null!;
    public DbSet<NameTestRecord> NameTestTable { get; set; } = null!;
    public DbSet<ArrayTestRecord> ArrayTestTable { get; set; } = null!;
    public DbSet<ClassListTestRecord> ClassListTestTable { get; set; } = null!;
    public DbSet<ComplexTestRecord> ComplexTestTable { get; set; } = null!;
    
    // RecordInfo.DbSetNameSuffix 에 맞지 않음
    public DbSet<ComplexTestRecord> ComplexTestRecords { get; set; } = null!;
    public DbSet<ComplexTestRecord> ComplexTestList { get; set; } = null!;
    
    // RecordInfo.TypeNameSuffix 에 맞지 않음
    public DbSet<BadTestData> BadTestTable { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}