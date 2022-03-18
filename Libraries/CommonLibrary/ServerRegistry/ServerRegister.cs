using System.Threading.Tasks;
using CommonLibrary.Worker;

namespace CommonLibrary.ServerRegistry
{
    public sealed class ServerRegister : IWork
    {
        private readonly ServerRegistryClient client;

        public ServerRegister(ServerRegistryClient client)
        {
            this.client = client;
        }

        public async Task RunAsync()
        {
            await client.RegisterServerAsync();
        }
    }
}