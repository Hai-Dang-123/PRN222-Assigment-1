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
            try
            {
                var result = await _context.NewsArticle
                    .Where(n => n.NewsStatus == true)  // Lọc bài viết có NewsStatus là true
                    .Include(n => n.Tags)              // Nạp bảng Tags
                    .Include(n => n.CreatedBy)         // Nạp bảng CreatedBy (User)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 ERROR: {ex.Message}");
                throw; // Ném lỗi ra để xem StackTrace
            }
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
            var allIds = await _context.NewsArticle
                                       .AsNoTracking()
                                       .Select(n => n.NewsArticleId)
                                       .ToListAsync();

            var maxId = allIds
                         .Where(id => int.TryParse(id, out _)) // Lọc những ID hợp lệ
                         .Select(id => int.Parse(id)) // Chuyển sang số
                         .DefaultIfEmpty(0) // Nếu không có ID thì mặc định là 0
                         .Max() // Lấy số lớn nhất
                         .ToString(); // Chuyển về string

            return maxId;
        }


    }
}
