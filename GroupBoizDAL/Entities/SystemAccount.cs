using System.ComponentModel.DataAnnotations;

namespace GroupBoizDAL.Entities
{
    public class SystemAccount
    {
        [Key]
        public short AccountID { get; set; }

        [StringLength(100)]
        public string AccountName { get; set; }

        [StringLength(70)]
        public string AccountEmail { get; set; }

        public int? AccountRole { get; set; }

        [StringLength(70)]
        public string AccountPassword { get; set; }

        public virtual ICollection<NewsArticle> CreatedNewsArticles { get; set; } = new List<NewsArticle>();
        public virtual ICollection<NewsArticle> UpdatedNewsArticles { get; set; } = new List<NewsArticle>();
    }
}
