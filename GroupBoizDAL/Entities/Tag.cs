using System.ComponentModel.DataAnnotations;

namespace GroupBoizDAL.Entities
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }

        [StringLength(50)]
        public string TagName { get; set; }

        [StringLength(400)]
        public string Note { get; set; }

        public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}
