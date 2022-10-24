using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.DevRecords;
using StaticDataLibrary.DevRecords.TestRecords;

namespace StaticDataLibrary;

public class StaticDataContext : DbContext
{
    private const string DbFileName = "StaticData.db";

    public StaticDataContext(DbContextOptions<StaticDataContext> options) 
        : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, DbFileName);
    }

    public string DbPath { get; } = null!;

    public DbSet<TypeTestRecord> TypeTestTable { get; set; } = null!;
    public DbSet<GroupTestRecord> GroupTestTable { get; set; } = null!;
    public DbSet<GroupedItemTestRecord> GroupedItemTestTable { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}