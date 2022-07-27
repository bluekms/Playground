using AuthDb;
using CommonLibrary.Handlers;

namespace AuthServer.Handlers.Foo;

public sealed record AddFooCommand(string AccountId, AuthDb.Foo.FooCommand Cmd, int Value) : ICommand;

public sealed class AddFooHandler : ICommandHandler<AddFooCommand>
{
    private readonly AuthDbContext dbContext;

    public AddFooHandler(AuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task ExecuteAsync(AddFooCommand command)
    {
        await dbContext.Foos.AddAsync(new()
        {
            AccountId = command.AccountId,
            Command = command.Cmd.ToString(),
            Value = command.Value,
        });

        await dbContext.SaveChangesAsync();
    }
}