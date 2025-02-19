using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;
using GroupBoizDAL.UnitOfWork;

namespace GroupBoizBLL.Services.Implement
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetAllNewsWithTag()
        {
            try
            {
                // Lấy danh sách tin tức từ cơ sở dữ liệu
                var newsList = await _unitOfWork.NewsArticleRepo.GetAllWithTagAsync();

                // Kiểm tra nếu không có tin tức nào
                if (newsList == null || !newsList.Any())
                {
                    return new ResponseDTO("No news found", 404, false); // Nếu không có tin tức
                }
                foreach (var news in newsList)
                {
                    Console.WriteLine($"ID: {news.NewsArticleId}, Status: {news.NewsStatus}");
                }


                return new ResponseDTO("News found successfully", 200, true, newsList); // Trả về danh sách tin tức
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false); // Nếu có lỗi, trả về thông báo lỗi và mã 500
            }
        }

        
        public async Task<ResponseDTO> GetNewsById(string NewsArticleId)
        {
            try
            {
                var news = await _unitOfWork.NewsArticleRepo.GetNewArticleByIdWithTagAsync(NewsArticleId);

                // Kiểm tra nếu không tìm thấy bài viết
                if (news == null)
                {
                    return new ResponseDTO("News not found", 404, false);
                }
                Console.WriteLine(news);
                return new ResponseDTO("News found successfully", 200, true, news);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }


        // ✅ Thêm phương thức cập nhật tin tức
        public async Task<ResponseDTO> UpdateNewsArticle(NewsArticle updatedNews)
        {
            try
            {
                if (updatedNews == null)
                {
                    return new ResponseDTO("Invalid news article data", 400, false);
                }

                var existingNews = await _unitOfWork.NewsArticleRepo.GetNewArticleByIdWithTagAsync(updatedNews.NewsArticleId);

                if (existingNews == null)
                {
                    return new ResponseDTO("News not found", 404, false);
                }

                // ✅ Cập nhật thông tin bài viết
                existingNews.NewsTitle = !string.IsNullOrEmpty(updatedNews.NewsTitle) ? updatedNews.NewsTitle : existingNews.NewsTitle;
                existingNews.NewsContent = !string.IsNullOrEmpty(updatedNews.NewsContent) ? updatedNews.NewsContent : existingNews.NewsContent;

                //// ✅ Cập nhật danh mục (Category)
                //if (updatedNews.Category != null && !string.IsNullOrEmpty(updatedNews.Category.CategoryName))
                //{
                //    var category = await _unitOfWork.CategoryRepo.GetCategoryByNameAsync(updatedNews.Category.CategoryName);
                //    if (category != null)
                //    {
                //        existingNews.CategoryId = category.CategoryId;
                //    }
                //}

                // ✅ Cập nhật danh sách thẻ (Tags)
                //if (updatedNews.NewsTags != null && updatedNews.NewsTags.Count > 0)
                //{
                //    var updatedTagNames = updatedNews.NewsTags.Select(t => t.Tag.TagName).ToList();
                //    var existingTags = existingNews.NewsTags.Select(nt => nt.Tag.TagName).ToList();

                //    var newTags = updatedTagNames.Except(existingTags).ToList();
                //    var removedTags = existingTags.Except(updatedTagNames).ToList();

                //    // Xóa thẻ không còn tồn tại
                //    existingNews.NewsTags.RemoveAll(nt => removedTags.Contains(nt.Tag.TagName));

                //    // Thêm thẻ mới
                //    foreach (var tagName in newTags)
                //    {
                //        var tag = await _unitOfWork.TagRepo.GetTagByNameAsync(tagName);
                //        if (tag == null)
                //        {
                //            tag = new Tag { TagName = tagName };
                //            await _unitOfWork.TagRepo.AddAsync(tag);
                //        }
                //        existingNews.NewsTags.Add(new NewsTag { TagId = tag.TagId, NewsArticleId = existingNews.NewsArticleId });
                //    }
                //}

                // ✅ Lưu thay đổi
                await _unitOfWork.NewsArticleRepo.UpdateAsync(existingNews);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("News updated successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }
        public async Task<ResponseDTO> DeleteNews(string newsArticleId)
        {
            try
            {
                if (newsArticleId == null)
                {
                    return new ResponseDTO("Invalid news article ID", 400, false);
                }

                var existingNews = await _unitOfWork.NewsArticleRepo.GetNewArticleByIdWithTagAsync(newsArticleId);

                if (existingNews == null)
                {
                    return new ResponseDTO("News not found", 404, false);
                }

                // ✅ Xóa bài viết
                await _unitOfWork.NewsArticleRepo.DeleteNewsAsync(existingNews);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("News deleted successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }


        public async Task<ResponseDTO> GetNewsByPeriod(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                // Lấy danh sách tin tức từ cơ sở dữ liệu
                var newsList = await _unitOfWork.NewsArticleRepo.GetAllWithTagAsync();

                // Kiểm tra nếu không có tin tức nào
                if (newsList == null || !newsList.Any())
                {
                    return new ResponseDTO("No news found", 404, false);
                }

                // Lọc theo khoảng thời gian
                var filteredNews = new List<NewsArticle>();
                foreach (var news in newsList)
                {
                    if ((!startDate.HasValue || news.CreatedDate >= startDate.Value) &&
                        (!endDate.HasValue || news.CreatedDate <= endDate.Value))
                    {
                        filteredNews.Add(news);
                    }
                }

                // Sắp xếp theo CreatedDate giảm dần
                filteredNews.Sort((a, b) => (b.CreatedDate ?? DateTime.MinValue)
                            .CompareTo(a.CreatedDate ?? DateTime.MinValue));

                // Kiểm tra nếu danh sách sau lọc trống
                if (!filteredNews.Any())
                {
                    return new ResponseDTO("No news found in the given period", 404, false);
                }

                return new ResponseDTO("News found successfully", 200, true, filteredNews);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }


    }
}
