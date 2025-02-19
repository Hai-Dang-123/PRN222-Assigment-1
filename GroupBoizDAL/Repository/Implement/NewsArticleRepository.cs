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
                //.Where(n => n.NewsStatus == true)
                .Include(n => n.Tags)   // Nạp bảng NewsTags
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
        public async Task DeleteNewsAsync(NewsArticle newsArticle)
        {
            _context.NewsArticle.Remove(newsArticle);
            await Task.CompletedTask;
        }



    }
}
