using Microsoft.EntityFrameworkCore;

namespace StaticDataLibrary.DevRecords;

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

    public DbSet<ArrayTestRecord> ArrayTestTable { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}