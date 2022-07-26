using System;
using CommonLibrary.Models;
using CommonLibrary.Worker;
using Microsoft.Extensions.Options;

namespace CommonLibrary.ServerRegistry
{
    public sealed class ServerRegisterOptionProvider : IWorkServiceOptions<ServerRegister>
    {
        public ServerRegisterOptionProvider(IOptions<ServerRegistryOptions> options)
        {
            Interval = TimeSpan.FromSeconds(options.Value.ExpireSec);
        }

        public TimeSpan Delay { get; } = TimeSpan.Zero;

        public TimeSpan Interval { get; }
    }
}