namespace DefaultNamespace;

// https://learn.microsoft.com/ko-kr/ef/core/get-started/overview/first-app?tabs=netcore-cli
public sealed class StaticDataContext : DbContext
{
    private const DbFileName = "StaticData.db";
    
    public DbSet<TestRecord> TestRecords { get; set; }
    
    public string DbPath { get; }

    public StaticDataContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "DbFileName");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}