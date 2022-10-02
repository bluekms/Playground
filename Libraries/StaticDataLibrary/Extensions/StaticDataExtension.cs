using System.Collections;
using System.Data.SQLite;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StaticDataLibrary.Records;

namespace StaticDataLibrary.Extensions;

public static class StaticDataExtension
{
    private static string StaticDataPath = "StaticData";
    private const string Extension = ".csv";
    
    public static async void UseStaticData(this IServiceCollection services)
    {
        services.AddEntityFrameworkSqlite().AddDbContext<StaticDataContext>();

        await using var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<StaticDataContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        AddTargetTestRecords2(context);
        
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);

        var recordList = RecordFinder.Find<StaticDataContext>();
        foreach (var ri in recordList)
        {
            var fileName = Path.Combine(path, StaticDataPath, $"{ri.SheetName}{Extension}");
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }

            var list = CreateList(ri.RecordType);

            using var reader = new StreamReader(fileName, Encoding.UTF8);
            while (!reader.EndOfStream)
            {
                var csvLine = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(csvLine))
                {
                    break;
                }

                var record = RecordMapper.Map(ri.RecordType, csvLine);
                list.Add(record);
            }
        }

        await context.SaveChangesAsync();
    }

    private static IList CreateList(Type t)
    {
        var values = Array.CreateInstance(t, 0);
        var genericListType = typeof(List<>);
        var concreteListType = genericListType.MakeGenericType(t);
        return (Activator.CreateInstance(concreteListType, values) as IList)!;
    }

    // TODO use transaction https://link2me.tistory.com/922
    private static async void AddTargetTestRecords(StaticDataContext context)
    {
        var query = "INSERT INTO TargetTestRecords VALUES (@Id, @Value1, @Value3)";

        await using var conn = new SqliteConnection(context.Database.GetConnectionString());
        
        var insertSql = new SqliteCommand(query, conn);
        insertSql.Parameters.Add(new("@Id", "1003"));
        insertSql.Parameters.Add(new("@Value1", "888"));
        insertSql.Parameters.Add(new("@Value3", "666"));

        await conn.OpenAsync();
        await insertSql.ExecuteNonQueryAsync();
    }
    
    private static async void AddTargetTestRecords2(StaticDataContext context)
    {
        var query = RecordQueryBuilder.InsertQuery(typeof(TargetTestRecord), "TargetTestRecords", out var parameters);

        await using var conn = new SqliteConnection(context.Database.GetConnectionString());
        
        var insertSql = new SqliteCommand(query, conn);
        for (int i = 0; i < parameters.Count; ++i)
        {
            var value = i == 0 ? "1003" : (10 + i).ToString();
            insertSql.Parameters.Add(new(parameters[i], value));
        }

        await conn.OpenAsync();
        await insertSql.ExecuteNonQueryAsync();
    }
}