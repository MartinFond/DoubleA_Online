using System;

namespace API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
