using CommonLibrary.Handlers;
using StaticDataLibrary.Records;

namespace StaticDataLibrary.Handlers;

public sealed record AddStaticDataCommand() : ICommand;

public sealed class AddStaticDataHandler : ICommandHandler<AddStaticDataCommand>
{
    private readonly StaticDataContext context;

    public AddStaticDataHandler(StaticDataContext context)
    {
        this.context = context;
    }

    public async Task ExecuteAsync(AddStaticDataCommand command)
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
    }
}