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
                var newsList = await _unitOfWork.NewsArticleRepo.GetAllWithTagAsync();

                if (newsList == null || !newsList.Any())
                {
                    return new ResponseDTO("No news found", 404, false);
                }
           

                // Mapping dữ liệu từ Entity -> DTO
                var newsDtoList = newsList.Select(news => new NewsArticleDTO
                {
                    NewsArticleId = news.NewsArticleId,
                    NewsTitle = news.NewsTitle,
                    Headline = news.Headline,
                    CreatedDate = news.CreatedDate,
                    NewsContent = news.NewsContent,
                    NewsSource = news.NewsSource,
                    CategoryId = news.CategoryId,
                    CategoryName = news.Category?.CategoryName ?? "Uncategorized",
                    NewsStatus = news.NewsStatus,
                    CreatedById = news.CreatedById,
                    CreateBy = news.CreatedBy?.AccountName ?? "Admin",
                    UpdatedById = news.UpdatedById,
                  
                    ModifiedDate = news.ModifiedDate,
                    ImageUrl = news.ImageUrl,
                    Tag = news.Tags?.Select(t => t.TagName).ToList() ?? new List<string>()  // ✅ Mapping danh sách Tags
                }).ToList();

                return new ResponseDTO("News found successfully", 200, true, newsDtoList);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
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
        public async Task<ResponseDTO> UpdateNewsArticle(NewsArticleDTO updatedNewsDto)
        {
            try
            {

                // Lấy bài báo từ database bằng NewsArticleId
                var existingNews = await _unitOfWork.NewsArticleRepo.GetNewArticleByIdWithTagAsync(updatedNewsDto.NewsArticleId);

                if (existingNews == null)
                {
                    return new ResponseDTO("News not found", 404, false);
                }

                // Cập nhật tiêu đề và nội dung nếu có thay đổi
                existingNews.NewsTitle = updatedNewsDto.NewsTitle ?? existingNews.NewsTitle;
                existingNews.NewsContent = updatedNewsDto.NewsContent ?? existingNews.NewsContent;

                //// Cập nhật CategoryId nếu có thay đổi
                //existingNews.CategoryId = updatedNewsDto.CategoryId ?? existingNews.CategoryId;

                // Cập nhật các trường khác (nếu có) như Headline, NewsSource, ModifiedDate, ImageUrl...
                //existingNews.Headline = updatedNewsDto.Headline ?? existingNews.Headline;
                //existingNews.NewsSource = updatedNewsDto.NewsSource ?? existingNews.NewsSource;
                //existingNews.ModifiedDate = updatedNewsDto.ModifiedDate ?? existingNews.ModifiedDate;
                //existingNews.ImageUrl = updatedNewsDto.ImageUrl ?? existingNews.ImageUrl;

                // Nếu có danh sách TagId từ DTO, cập nhật danh sách tag của bài báo
                if (updatedNewsDto.TagId != null && updatedNewsDto.TagId.Any())
                {
                    // Xóa tất cả các tag cũ
                    existingNews.Tags.Clear();

                    // Lấy danh sách tag mới từ database theo TagId
                    var newTags = await _unitOfWork.TagRepo.GetTagsByIdsAsync(updatedNewsDto.TagId);

                    if (newTags != null && newTags.Any())
                    {
                        // Gán lại danh sách tag mới
                        foreach (var tag in newTags)
                        {
                            existingNews.Tags.Add(tag);
                        }
                    }
                }

                // Lưu bài báo đã cập nhật vào database
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
                existingNews.NewsStatus = false;
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("News deleted successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }
        public async Task<ResponseDTO> SearchNewsByTitle(string title)
        {
            try
            {
                Console.WriteLine($"🔍 Searching news with title: {title}");

                var newsList = await _unitOfWork.NewsArticleRepo.SearchByTitleAsync(title);

                if (newsList == null || !newsList.Any())
                {
                    Console.WriteLine("⚠️ No news found!");
                    return new ResponseDTO("No news found with the given title", 404, false);
                }

                Console.WriteLine($"✅ Found {newsList.Count} news articles");

                // Mapping từ Entity → DTO
                var newsDtoList = newsList.Select(news => new NewsArticleDTO
                {
                    NewsArticleId = news.NewsArticleId,
                    NewsTitle = news.NewsTitle,
                    Headline = news.Headline,
                    CreatedDate = news.CreatedDate,
                    NewsContent = news.NewsContent,
                    NewsSource = news.NewsSource,
                    CategoryId = news.CategoryId,
                    CategoryName = news.Category?.CategoryName ?? "Uncategorized",
                    NewsStatus = news.NewsStatus,
                    CreatedById = news.CreatedById,
                    CreateBy = news.CreatedBy?.AccountName ?? "Admin",
                    UpdatedById = news.UpdatedById,
                    ModifiedDate = news.ModifiedDate,
                    ImageUrl = news.ImageUrl,
                    Tag = news.Tags?.Select(t => t.TagName).ToList() ?? new List<string>()  // ✅ Mapping danh sách Tags
                }).ToList();

                Console.WriteLine($"📝 Mapped {newsDtoList.Count} news to DTO");

                return new ResponseDTO("News found successfully", 200, true, newsDtoList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> GetByCategoryAsync(int categoryId)
        {
            try
            {
                Console.WriteLine($"🔍 Fetching news by category ID: {categoryId}");

                var newsList = await _unitOfWork.NewsArticleRepo.GetByCategoryAsync(categoryId);

                if (newsList == null || !newsList.Any())
                {
                    Console.WriteLine($"⚠️ No news found for category {categoryId}");
                    return new ResponseDTO("No news found in this category", 404, false);
                }

                Console.WriteLine($"✅ Found {newsList.Count} news articles in category {categoryId}");

                // Mapping từ Entity → DTO
                var newsDtoList = newsList.Select(news => new NewsArticleDTO
                {
                    NewsArticleId = news.NewsArticleId,
                    NewsTitle = news.NewsTitle,
                    Headline = news.Headline,
                    CreatedDate = news.CreatedDate,
                    NewsContent = news.NewsContent,
                    NewsSource = news.NewsSource,
                    CategoryId = news.CategoryId,
                    CategoryName = news.Category?.CategoryName ?? "Uncategorized",
                    NewsStatus = news.NewsStatus,
                    CreatedById = news.CreatedById,
                    CreateBy = news.CreatedBy?.AccountName ?? "Admin",
                    UpdatedById = news.UpdatedById,
                    ModifiedDate = news.ModifiedDate,
                    ImageUrl = news.ImageUrl,
                    Tag = news.Tags?.Select(t => t.TagName).ToList() ?? new List<string>()  // ✅ Mapping danh sách Tags
                }).ToList();

                Console.WriteLine($"📝 Mapped {newsDtoList.Count} news to DTO");

                return new ResponseDTO("News found successfully", 200, true, newsDtoList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> GetByTagAsync(int tagId)
        {
            try
            {
                Console.WriteLine($"🔍 Fetching news by tag ID: {tagId}");

                var newsList = await _unitOfWork.NewsArticleRepo.GetByTagAsync(tagId);

                if (newsList == null || !newsList.Any())
                {
                    Console.WriteLine($"⚠️ No news found for tag {tagId}");
                    return new ResponseDTO("No news found with this tag", 404, false);
                }

                Console.WriteLine($"✅ Found {newsList.Count} news articles with tag {tagId}");

                // Mapping từ Entity → DTO
                var newsDtoList = newsList.Select(news => new NewsArticleDTO
                {
                    NewsArticleId = news.NewsArticleId,
                    NewsTitle = news.NewsTitle,
                    Headline = news.Headline,
                    CreatedDate = news.CreatedDate,
                    NewsContent = news.NewsContent,
                    NewsSource = news.NewsSource,
                    CategoryId = news.CategoryId,
                    CategoryName = news.Category?.CategoryName ?? "Uncategorized",
                    NewsStatus = news.NewsStatus,
                    CreatedById = news.CreatedById,
                    CreateBy = news.CreatedBy?.AccountName ?? "Admin",
                    UpdatedById = news.UpdatedById,
                    ModifiedDate = news.ModifiedDate,
                    ImageUrl = news.ImageUrl,
                    Tag = news.Tags?.Select(t => t.TagName).ToList() ?? new List<string>()  // ✅ Mapping danh sách Tags
                }).ToList();

                Console.WriteLine($"📝 Mapped {newsDtoList.Count} news to DTO");

                return new ResponseDTO("News found successfully", 200, true, newsDtoList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}");
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
        public async Task<ResponseDTO> CreateNewsArticle(NewsArticleDTO newsDto)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(newsDto.NewsTitle) || string.IsNullOrWhiteSpace(newsDto.NewsContent))
                {
                    return new ResponseDTO("NewsTitle and NewsContent are required", 400, false);
                }

                // Kiểm tra CreatedById có tồn tại không
                var createById = newsDto.CreatedById.Value;
                Console.WriteLine(createById.ToString());
                var createBy = await _unitOfWork.AccountRepo.GetByShortIdAsync(createById);

                if (createBy == null)
                {
                    return new ResponseDTO("Error: The provided CreatedById does not exist in SystemAccount.", 400, false);
                }

                var maxId = await _unitOfWork.NewsArticleRepo.GetMaxNewsArticleId();



                string newIdNumber = string.IsNullOrEmpty(maxId) ? "1" : (int.Parse(maxId) + 1).ToString();

                // Tạo đối tượng NewsArticle
                var newsArticle = new NewsArticle
                {
                    NewsArticleId = newIdNumber,
                    NewsTitle = newsDto.NewsTitle,
                    Headline = newsDto.Headline,
                    CreatedDate = DateTime.Now,
                    NewsContent = newsDto.NewsContent,
                    NewsSource = newsDto.NewsSource,
                    CategoryId = newsDto.CategoryId,
                    CreatedById = newsDto.CreatedById,
                    CreatedBy = createBy,
                    NewsStatus = true,
                    ModifiedDate = newsDto.ModifiedDate,
                    ImageUrl = newsDto.ImageUrl
                };

                // Lưu bài báo vào database trước
                await _unitOfWork.NewsArticleRepo.AddAsync(newsArticle);
                await _unitOfWork.SaveChangeAsync(); // ✅ Lưu ngay để có ID

                // Nếu có danh sách TagId, lấy từ DB và thêm vào bài báo
                if (newsDto.TagId != null && newsDto.TagId.Any())
                {
                    var tags = await _unitOfWork.TagRepo.GetTagsByIdsAsync(newsDto.TagId);
                    if (tags != null && tags.Any())
                    {
                        newsArticle.Tags = tags.ToList();
                        await _unitOfWork.SaveChangeAsync(); // ✅ Lưu lại sau khi gán Tags
                    }
                }

                return new ResponseDTO("News created successfully", 201, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }


    }
}
