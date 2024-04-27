using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Achievements", Schema = "public")]
    public class Achievement
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name")]
        public required string Name { get; set; }
        [Column("description")]
        public required string Desc { get; set; }
        [Column("image_url")]
        public required string ImgURL { get; set; }
    }
}