using System.Threading.Tasks;
using AuthDb;
using CommonLibrary.Handlers;

namespace AuthServer.Handlers.Foo
{
    public sealed record AddFooCommand(string AccountId, AuthDb.Foo.FooCommand Cmd, int Value) : ICommand;

    public sealed class AddFooHandler : ICommandHandler<AddFooCommand>
    {
        private readonly AuthContext _context;

        public AddFooHandler(AuthContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(AddFooCommand command)
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