using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TG.Models
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
    }
}
