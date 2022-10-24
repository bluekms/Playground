using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.DevRecords;
using StaticDataLibrary.DevRecords.TestRecords;

namespace StaticDataLibrary.Test;

public class TestStaticDataContext : DbContext
{
    public const int TestTableCount = 8;
    
    private const string DbFileName = "TestStaticData.db";

    public TestStaticDataContext(DbContextOptions<TestStaticDataContext> options)
        : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, DbFileName);
    }
    
    public string DbPath { get; } = null!;

    public DbSet<TypeTestRecord> TypeTestTable { get; set; } = null!;
    public DbSet<TargetTestRecord> TargetTestTable { get; set; } = null!;
    public DbSet<NameTestRecord> NameTestTable { get; set; } = null!;
    public DbSet<ArrayTestRecord> ArrayTestTable { get; set; } = null!;
    public DbSet<ClassListTestRecord> ClassListTestTable { get; set; } = null!;
    public DbSet<ComplexTestRecord> ComplexTestTable { get; set; } = null!;
    public DbSet<GroupedItemTestRecord> GroupedItemTestTable { get; set; } = null!;
    public DbSet<GroupTestRecord> GroupTestTable { get; set; } = null!;
    
    // RecordInfo.DbSetNameSuffix 에 맞지 않음
    public DbSet<BadTestData> ComplexTestRecords { get; set; } = null!;
    public DbSet<BadTestData> ComplexTestList { get; set; } = null!;
    
    // RecordInfo.TypeNameSuffix 에 맞지 않음
    public DbSet<BadTestData> BadTestTable { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}