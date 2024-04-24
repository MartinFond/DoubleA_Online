using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("User", Schema = "public")]
    public class User
    {
         [Column("id")]
        public Guid Id { get; set; }
         [Column("username")]
        public required string Username { get; set; }
         [Column("password")]
        public required string Password { get; set; }
         [Column("salt")]
        public required string Salt { get; set; }
         [Column("role_id")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        [Column("email")]
        public required string Email {get; set;}
    }
}
