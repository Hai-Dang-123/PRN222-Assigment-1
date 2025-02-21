using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizDAL.Data;
using GroupBoizDAL.Entities;
using GroupBoizDAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using static GroupBoizDAL.Repository.Interface.INewsArticleRepository;

namespace GroupBoizDAL.Repository.Implement
{
    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        private readonly FUNewsManagementContext _context;
        public NewsArticleRepository(FUNewsManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<NewsArticle>> GetAllWithTagAsync()
        {
            return await _context.NewsArticle
                .Where(n => n.NewsStatus == true)  // Lọc bài viết có NewsStatus là true
                .Include(n => n.Tags)              // Nạp bảng NewsTags
                .Include(n => n.CreatedBy)
                .ToListAsync();
        }

        public async Task<NewsArticle?> GetNewArticleByIdWithTagAsync(string id)
        {
            return await _context.NewsArticle
                .Include(n => n.Tags) // Load danh sách Tags liên quan
                .Include(n => n.CreatedBy) // Load thông tin người tạo
                .Include(n => n.Category)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);
        }
        //public async Task DeleteNewsAsync(NewsArticle newsArticle)
        //{
        //    _context.NewsArticle.Remove(newsArticle);
        //    await Task.CompletedTask;
        //}


        public async Task<List<NewsArticle>> SearchByTitleAsync(string title)
        {
            return await _context.NewsArticle
                .Include(n => n.Category) // Load category của bài viết
                .Include(n => n.Tags) // Load tags của bài viết
                .Include(n => n.CreatedBy) // Load thông tin người tạo bài viết
                .Where(n => n.NewsTitle.Contains(title))
                .ToListAsync();
        }
        public async Task<List<NewsArticle>> GetByCategoryAsync(int categoryId)
        {

            return await _context.NewsArticle
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Include(n => n.CreatedBy)
                .Where(n => n.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<NewsArticle>> GetByTagAsync(int tagId)
        {
            return await _context.NewsArticle
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Include(n => n.CreatedBy)
                .Where(n => n.Tags.Any(t => t.TagId == tagId))
                .ToListAsync();
        }

        public async Task CreateNewsArticle(NewsArticle newsArticle, List<int> selectedTags)
        {
            if (newsArticle == null)
                throw new ArgumentNullException(nameof(newsArticle));

           
            _context.NewsArticle.Add(newsArticle);
            await _context.SaveChangesAsync();

            
        }
        public async Task<string> GetMaxNewsArticleId()
        {
            // Lấy ID lớn nhất trong database
            var maxId = await _context.NewsArticle
                                      .OrderByDescending(n => n.NewsArticleId)
                                      .Select(n => n.NewsArticleId)
                                      .FirstOrDefaultAsync();

            return maxId; // Trả về ID lớn nhất (hoặc null nếu chưa có bài viết nào)
        }




    }
}
