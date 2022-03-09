using System.Threading.Tasks;
using AuthDb;
using AuthServer.Models;
using CommonLibrary;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using MapsterMapper;

namespace AuthServer.Handlers.Account
{
    public sealed record AddAccountCommand(
        string AccountId,
        string Password,
        UserRoles UserRole) : ICommand;

    public sealed class AddAccountHandler : ICommandHandler<AddAccountCommand, AccountData>
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;
        private readonly ITimeService time;

        public AddAccountHandler(
            AuthContext context,
            IMapper mapper,
            ITimeService time)
        {
            this.context = context;
            this.mapper = mapper;
            this.time = time;
        }

        public async Task<AccountData> ExecuteAsync(AddAccountCommand command)
        {
            var newRow = new AuthDb.Account()
            {
                AccountId = command.AccountId,
                Password = command.Password,
                Token = string.Empty,
                CreatedAt = time.Now,
                Role = command.UserRole,
            };

            await context.Accounts.AddAsync(newRow);
            await context.SaveChangesAsync();

            return mapper.Map<AccountData>(newRow);
        }
    }
}