using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBoizCommon.DTO
{
    public class NewsArticleDTO
    {
        public string NewsArticleId { get; set; } = null!;


        public string? NewsTitle { get; set; }


        public string Headline { get; set; } = null!;


        public DateTime? CreatedDate { get; set; }


        public string? NewsContent { get; set; }


        public string? NewsSource { get; set; }


        public short? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public bool? NewsStatus { get; set; }



        public short? CreatedById { get; set; }
        public string? CreateBy { get; set; }

        public short? UpdatedById { get; set; }
        public string? UpdateBy { get; set; }


        public DateTime? ModifiedDate { get; set; }

        public string? ImageUrl { get; set; }


        public List<string>? Tag { get; set; }
        public List<int> TagId { get; set; }
        
    }
}
