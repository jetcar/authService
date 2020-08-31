using System;

namespace Repository.DbModel
{
    public class SessionDb
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

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}