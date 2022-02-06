using System;
using System.Diagnostics.CodeAnalysis;

namespace AuthDb
{
    public sealed class World
    {
        [AllowNull]
        public string WorldName { get; set; }
        
        [AllowNull]
        public string WorldType { get; set; }
        
        [AllowNull]
        public string Address { get; set; }
        
        public DateTime ExpireAt { get; set; }
    }
}