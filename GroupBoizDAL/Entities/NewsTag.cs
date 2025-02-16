using Azure;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GroupBoizDAL.Entities
{
    public class NewsTag
    {
        [Key, Column(Order = 0)]
        [StringLength(20)]
        public string NewsArticleID { get; set; }

        [Key, Column(Order = 1)]
        public int TagID { get; set; }

        [ForeignKey("NewsArticleID")]
        public virtual NewsArticle NewsArticle { get; set; }

        [ForeignKey("TagID")]
        public virtual Tag Tag { get; set; }
    }
}
