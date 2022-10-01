using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StaticDataLibrary.Records;

namespace StaticDataLibrary.Extensions;

public static class StaticDataExtension
{
    public static void UseStaticData(this IServiceCollection services)
    {
        services.AddEntityFrameworkSqlite().AddDbContext<StaticDataContext>();
        
        using var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<StaticDataContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        AddTargetTestRecords(context);
        
        // TODO 다른 테이블들도 여기서 추가할 수 있을까?
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

        await context.TargetTestRecords.AddRangeAsync(targetTestList);
        await context.SaveChangesAsync();
    }
}