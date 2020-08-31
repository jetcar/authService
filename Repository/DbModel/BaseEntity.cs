using System;

namespace Repository.DbModel
{
    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}