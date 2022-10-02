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

        // StaticDataContext를 리플랙션으로 가져와서 Record 타입들을 가져온다
        // Record 타입들의 이름 혹은 Attribute를 가져와서 파일명을 알아낸다
        // 열어서 불러온다
        // EF Core를 사용하면 너무 느리다? 나중에 시간 비교해보자

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
            
            // TODO Fail
            
            var foo = list as List<TargetTestRecord>;
            
            var propertyInfo = RecordFinder.Find<StaticDataContext>(ri.DbSetName);
            var methodInfo = propertyInfo.PropertyType.GetMethod("AddRange", new[] {list.GetType()});

            //context.TargetTestRecords.AddRange(foo);
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

    private static async void AddTargetTestRecords(StaticDataContext context)
    {
        var targetTestList = new List<TargetTestRecord>();
        for (int i = 0; i < 10; ++i)
        {
            var record = new TargetTestRecord()
            {
                Id = 1000 + i,
                Value1 = i,
                Value3 = i * 3,
            };
            
            targetTestList.Add(record);
        }
        
        context.TargetTestRecords.AddRange(targetTestList);

        await context.TargetTestRecords.AddRangeAsync(targetTestList);
        await context.SaveChangesAsync();
    }
    
    private static async void AddTargetTestRecords2(StaticDataContext context)
    {
        var query = "INSERT INTO TargetTestRecords (Id, Value1, Value3) VALUES (@Id, @Value1, @Value3)";

        await using var conn = new SqliteConnection(context.Database.GetConnectionString());
        
        var insertSql = new SqliteCommand(query, conn);
        insertSql.Parameters.Add(new("@Id", 1003));
        insertSql.Parameters.Add(new("@Value1", 888));
        insertSql.Parameters.Add(new("@Value3", 666));

        await conn.OpenAsync();
        await insertSql.ExecuteNonQueryAsync();
    }
}