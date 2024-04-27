using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("role", Schema = "public")]
    public class Role
    {
        [Column("id")]
        public RoleType Id { get; set; }
    }
}
