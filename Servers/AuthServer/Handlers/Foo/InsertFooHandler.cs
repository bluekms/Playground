using System.Threading.Tasks;
using AuthDb;
using CommonLibrary.Handlers;

namespace AuthServer.Handlers.Foo
{
    public sealed record InsertFooCommand(string AccountId, AuthDb.Foo.FooCommand Cmd, int Value) : ICommand;

    public sealed class InsertFooHandler : ICommandHandler<InsertFooCommand>
    {
        private readonly AuthContext _context;

        public InsertFooHandler(AuthContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(InsertFooCommand command)
        {
            await _context.Foos.AddAsync(new()
            {
                AccountId = command.AccountId,
                Command = command.Cmd.ToString(),
                Value = command.Value,
            });

            await _context.SaveChangesAsync();
        }
    }
}