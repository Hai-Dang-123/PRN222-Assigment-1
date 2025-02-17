using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupBoizDAL.Entities
{
    public class Tag
    {
        [Key]
        [Column("TagID")]
        public int TagId { get; set; }

        [StringLength(50)]
        public string? TagName { get; set; }

        [StringLength(400)]
        public string? Note { get; set; }

        [ForeignKey("TagId")]
        [InverseProperty("Tags")]
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    }
}
