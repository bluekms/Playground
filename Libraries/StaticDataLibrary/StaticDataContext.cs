using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.DevRecords;

namespace StaticDataLibrary;

public class StaticDataContext : DbContext
{
    public StaticDataContext(DbContextOptions<StaticDataContext> options, string dbFileName = "StaticData.db") 
        : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, dbFileName);
    }

    public string DbPath { get; } = null!;

    public DbSet<TypeTestRecord> TypeTestTable { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}