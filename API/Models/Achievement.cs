using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("achievement", Schema = "public")]
    public class Achievement
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name")]
        public required string Name { get; set; }
        [Column("desc")]
        public required string Desc { get; set; }
        [Column("imgurl")]
        public required string ImgURL { get; set; }
    }
}