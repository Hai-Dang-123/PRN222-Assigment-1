using GroupBoizBLL.Services.Interface;
using GroupBoizDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly INewsArticleService _newsService;

        public SearchController(INewsArticleService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                ViewBag.News = new List<NewsArticle>(); // Trả về danh sách rỗng nếu không có từ khóa
                return View();
            }

            // Gọi service để tìm kiếm bài viết theo tiêu đề
            var results = await _newsService.SearchNewsByTitle(q);
            ViewBag.News = results;

            return View("Index");
        }
    }
}
