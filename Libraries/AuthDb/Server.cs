using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using CommonLibrary.Models;

namespace AuthDb
{
    public sealed class Server
    {
        [AllowNull]
        [Key]
        public string Name { get; init; }

        public ServerRoles Role { get; set; }

        [AllowNull]
        public string Address { get; set; }

        public DateTime ExpireAt { get; set; }
        
        [AllowNull]
        public string Description { get; set; }
    }
}