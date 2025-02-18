using GroupBoizDAL.Data;
using GroupBoizDAL.Entities;
using GroupBoizDAL.Repository.Interface;

namespace GroupBoizDAL.Repository.Implement
{
    public class NewsRepository : GenericRepository<NewsArticle>, INewsRepository
    {
        private readonly FUNewsManagementContext _context;

        public NewsRepository(FUNewsManagementContext context) : base(context)
        {
            _context = context;
        }

        public List<NewsArticle> GetAllNews()
        {
            return _context.NewsArticle.ToList();
        }

        public NewsArticle GetById(string id)
        {
            return _context.NewsArticle.FirstOrDefault(n => n.NewsArticleId == id);
        }

        public void Update(NewsArticle news)
        {
            _context.NewsArticle.Update(news);
            _context.SaveChanges();
        }
    }
}
