using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using CommonLibrary.Models;

namespace AuthDb
{
    public sealed class Account
    {
        [AllowNull]
        [Key]
        public string AccountId { get; init; }
        
        [AllowNull]
        public string Password { get; init; }
        
        public DateTime CreatedAt { get; set; }
        
        [AllowNull]
        public string Token { get; set; }
        
        [AllowNull]
        public UserRoles Role { get; set; }
    }
}