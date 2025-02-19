using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizDAL.Entities;

namespace GroupBoizDAL.Repository.Interface
{
    public interface INewsArticleRepository : IGenericRepository<NewsArticle>
    {
        Task<List<NewsArticle>> GetAllWithTagAsync();
        Task<NewsArticle?> GetNewArticleByIdWithTagAsync(string id);
        Task DeleteNewsAsync(NewsArticle newsArticle);

        Task<List<NewsArticle>> SearchByTitleAsync(string title);
        Task<List<NewsArticle>> GetByCategoryAsync(int categoryId);
        Task<List<NewsArticle>> GetByTagAsync(int tagId);
    }
}
