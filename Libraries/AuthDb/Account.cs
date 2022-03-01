using System;
using System.Diagnostics.CodeAnalysis;
using CommonLibrary.Models;

namespace AuthDb
{
    public sealed class Account
    {
        [AllowNull]
        public string AccountId { get; set; }
        
        [AllowNull]
        public string Password { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        [AllowNull]
        public string Token { get; set; }
        
        [AllowNull]
        public UserRoles Role { get; set; }
    }
}