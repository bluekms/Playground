using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using WorldServer;

namespace WorldServer.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> logger;

        public GreeterService(ILogger<GreeterService> logger)
        {
            this.logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name,
            });
        }
    }
}