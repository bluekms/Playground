using System.Diagnostics.CodeAnalysis;
using CommonLibrary.Models;

namespace AuthDb
{
    public class Credential
    {
        [AllowNull]
        public string Token { get; set; }
        
        [AllowNull]
        public string Name { get; set; }
        
        public ServerRoles Role { get; set; }
        
        [AllowNull]
        public string Description { get; set; }
    }
}