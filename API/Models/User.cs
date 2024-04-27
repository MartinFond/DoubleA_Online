using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace API.Models
{
    public enum RoleType 
    {
        [Display(Name = "None")]
        None,
        [Display(Name = "Player")]
        Player,
        [Display(Name = "DGS")]
        DGS
    }

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
         [Column("role")]
        public RoleType Role { get; set; }
        [Column("email")]
        public required string Email {get; set;}
    }
}
