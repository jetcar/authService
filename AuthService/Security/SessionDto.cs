using System;

namespace AuthService.Security
{
    public class SessionDto
    {
        public long SessionId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime ExpirationTime { get; set; }
        public int UserId { get; set; }
    }
}