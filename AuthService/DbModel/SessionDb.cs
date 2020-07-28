using System;

namespace AuthService.DbModel
{
    public class SessionDb : BaseEntity
    {
        public SessionDb()
        { }
        public SessionDb(int userId)
        {
            UserId = userId;
        }

        public long Id { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
    }

    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; }

    }
}