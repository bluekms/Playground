using System;
using System.Diagnostics.CodeAnalysis;
using CommonLibrary.Models;

namespace AuthDb
{
    public sealed class Server
    {
        [AllowNull]
        public string Name { get; set; }

        public ServerRoles Role { get; set; }

        [AllowNull]
        public string Address { get; set; }

        public DateTime ExpireAt { get; set; }
        
        [AllowNull]
        public string Description { get; set; }
    }
}