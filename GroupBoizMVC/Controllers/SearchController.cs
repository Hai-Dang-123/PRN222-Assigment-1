using GroupBoizBLL.Services.Interface;
using GroupBoizDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public SearchController(INewsArticleService newsService, ICategoryService categoryService, ITagService tagService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string q)
        {
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();
            ViewBag.Category = categoryResponse.Result;  // Truyền categories vào view
            ViewBag.Tag = tagResponse.Result;  // Truyền tags vào view
            if (string.IsNullOrWhiteSpace(q))
            {

                ViewBag.News = new List<NewsArticle>(); // Trả về danh sách rỗng nếu không có từ khóa
                return View();
            }


            // Gọi service để tìm kiếm bài viết theo tiêu đề
            var results = await _newsService.SearchNewsByTitle(q);
            ViewBag.News = results.Result;

            return View("Index");
        }
    }
}
