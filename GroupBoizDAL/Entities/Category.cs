using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupBoizDAL.Entities
{
    public class Category
    {
        [Key]
        public short CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [Required]
        [StringLength(250)]
        public string CategoryDesciption { get; set; }

        public short? ParentCategoryID { get; set; }

        public bool? IsActive { get; set; }

        [ForeignKey("ParentCategoryID")]
        public virtual Category ParentCategory { get; set; }

        public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    }
}
