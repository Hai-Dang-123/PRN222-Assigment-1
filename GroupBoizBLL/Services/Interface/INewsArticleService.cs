using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;

namespace GroupBoizBLL.Services.Interface
{
    public interface INewsArticleService
    {
        Task<ResponseDTO> GetAllNewsWithTag();
        Task<ResponseDTO> GetNewsById(string NewsArticleId);
        Task<ResponseDTO> UpdateNewsArticle(NewsArticle updatedNews);
        Task<ResponseDTO> DeleteNews(string newsArticleId);
        Task<List<NewsArticle>> SearchNewsByTitle(string title);
        Task<List<NewsArticle>> GetByCategoryAsync(int categoryId);
        Task<List<NewsArticle>> GetByTagAsync(int tagId);
        Task<ResponseDTO> GetNewsByPeriod(DateTime? startDate, DateTime? endDate);
    }
}
