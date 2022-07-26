using CommonLibrary.Handlers;

namespace WorldServer.Handlers.Foo;

public sealed record AddFooCommand(string Data) : ICommand;

public sealed class AddFooHandler : ICommandHandler<AddFooCommand>
{
    private readonly WorldDbContext dbContext;

    public AddFooHandler(WorldDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task ExecuteAsync(AddFooCommand command)
    {
        await dbContext.Foos.AddAsync(new()
        {
            Data = command.Data,
        });

        await dbContext.SaveChangesAsync();
    }
}