using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("role", Schema = "public")]
    public class Role
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public required string Name { get; set; }
    }
}
