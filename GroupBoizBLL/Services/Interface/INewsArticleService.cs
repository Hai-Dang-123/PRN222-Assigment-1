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
        Task<ResponseDTO> UpdateNewsArticle(NewsArticleDTO updatedNews);
        Task<ResponseDTO> DeleteNews(string newsArticleId);
        Task<ResponseDTO> SearchNewsByTitle(string title);
        Task<ResponseDTO> GetByCategoryAsync(int categoryId);
        Task<ResponseDTO> GetByTagAsync(int tagId);
        Task<ResponseDTO> GetNewsByPeriod(DateTime? startDate, DateTime? endDate);
        Task<ResponseDTO> CreateNewsArticle(NewsArticleDTO newsDto);

    }
}
