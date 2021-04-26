using System;

namespace TG.Entity
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public long TGId { get; set; }
        public string Status { get; set; }
    }
}
