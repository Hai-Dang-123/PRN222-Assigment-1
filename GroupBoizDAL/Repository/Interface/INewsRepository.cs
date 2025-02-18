using GroupBoizDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBoizDAL.Repository.Interface
{
    public interface INewsRepository : IGenericRepository<NewsArticle>
    {
        List<NewsArticle> GetAllNews(); // THÊM HÀM NÀY
        NewsArticle GetById(string id); // Lấy bài viết theo ID
        void Update(NewsArticle news); // Thêm phương thức cập nhật bài viết
    }
}
