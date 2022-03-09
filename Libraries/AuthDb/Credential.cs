using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using CommonLibrary.Models;

namespace AuthDb
{
    public class Credential
    {
        [AllowNull]
        [Key]
        public string Name { get; set; }
        
        [AllowNull]
        public string Token { get; set; }
        
        public ServerRoles Role { get; set; }
        
        [AllowNull]
        public string Description { get; set; }
    }
}