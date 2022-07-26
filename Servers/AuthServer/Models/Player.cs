using System;

namespace AuthServer.Models
{
    // TODO AccountContext에 record로 합치기
    public sealed class Player
    {
        public string PlayerId { get; set; } = string.Empty;

        public string AccountId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string Nickname { get; set; } = string.Empty;
    }
}