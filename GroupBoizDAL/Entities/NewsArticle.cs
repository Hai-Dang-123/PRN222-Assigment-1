using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GroupBoizDAL.Entities
{
    public class NewsArticle
    {
        [Key]
        [StringLength(20)]
        public string NewsArticleID { get; set; }

        [StringLength(400)]
        public string NewsTitle { get; set; }

        [Required]
        [StringLength(150)]
        public string Headline { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(4000)]
        public string NewsContent { get; set; }

        [StringLength(400)]
        public string NewsSource { get; set; }

        public short? CategoryID { get; set; }

        public bool? NewsStatus { get; set; }

        public short? CreatedByID { get; set; }
        public short? UpdatedByID { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        [ForeignKey("CreatedByID")]
        public virtual SystemAccount CreatedBy { get; set; }

        [ForeignKey("UpdatedByID")]
        public virtual SystemAccount UpdatedBy { get; set; }

        public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}
