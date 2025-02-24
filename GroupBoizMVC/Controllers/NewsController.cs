using GroupBoizBLL.Services.Interface;
using GroupBoizBLL.Utilities;
using GroupBoizCommon.DTO;


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroupBoizMVC.Controllers
{
    public class NewsController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly INewsArticleService _newsArticleService;
        private readonly UserUtility _userUtility;
        public NewsController (ICategoryService categoryService, ITagService tagService,  INewsArticleService newsArticleService, UserUtility userUtility)
        {
            _categoryService = categoryService;
            _tagService = tagService;
            _newsArticleService = newsArticleService;
            _userUtility = userUtility;
        }

        // Action để hiển thị danh sách Category
        public async Task<IActionResult> Index()
        {
            // Gọi phương thức GetAll từ CategoryService để lấy dữ liệu
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();
            var newsResponse = await _newsArticleService.GetAllNewsWithTag();
            //var userRole = User.FindFirstValue(ClaimTypes.Role); // Lấy role từ Claims
            var role = _userUtility.GetRoleFromToken();
            var userId = _userUtility.GetUserIDFromToken();
            Console.WriteLine(userId);
            Console.WriteLine(role);

            if (categoryResponse.IsSuccess && tagResponse.IsSuccess && newsResponse.IsSuccess)
            {
                ViewBag.Category = categoryResponse.Result;  // Truyền categories vào view
                ViewBag.Tag = tagResponse.Result;  // Truyền tags vào view
                ViewBag.News = newsResponse.Result;
                //ViewBag.UserRole = userRole;
                ViewBag.Role = role;

                return View(); // Trả về view chính
            }
            else
            {
                ViewBag.ErrorMessage = categoryResponse.Message ?? tagResponse.Message;
                ViewBag.TopThreeNews = new List<GroupBoizDAL.Entities.NewsArticle>();  // Tránh null reference
                return View();
            }
        }
        public async Task<IActionResult> ByCategory(int id)
        {
            var articles = await _newsArticleService.GetByCategoryAsync(id);
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();
            var role = _userUtility.GetRoleFromToken();
            ViewBag.Role = role;
            ViewBag.News = articles.Result;
            ViewBag.Category = categoryResponse.Result;  // Truyền categories vào view
            ViewBag.Tag = tagResponse.Result;  // Truyền tags vào view
            return View("Index");
        }

        public async Task<IActionResult> ByTag(int id)
        {
            var articles = await _newsArticleService.GetByTagAsync(id);
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();
            var role = _userUtility.GetRoleFromToken();
            ViewBag.Role = role;
            ViewBag.News = articles.Result;
            ViewBag.Category = categoryResponse.Result;  // Truyền categories vào view
            ViewBag.Tag = tagResponse.Result;  // Truyền tags vào view
            return View("Index");
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAll();
            var tags = await _tagService.GetAllTags();
            var role = _userUtility.GetRoleFromToken();
            ViewBag.Role = role;

            ViewBag.Category = categories.Result;
            ViewBag.Tag = tags.Result;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsArticleDTO newsArticle)
        {
            var categories = await _categoryService.GetAll();
            var tags = await _tagService.GetAllTags();
            var role = _userUtility.GetRoleFromToken();
            var userId = _userUtility.GetUserIDFromToken();
            Console.WriteLine(userId);
            ViewBag.Category = categories.Result;
            ViewBag.Tag = tags.Result;
            ViewBag.Role = role;

            if (!ModelState.IsValid)
            {
               
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.ErrorMessage = "Invalid input data! Errors: " + string.Join(", ", errors);
                return View(newsArticle);
            }

            newsArticle.CreatedById = userId; // ✅ Đảm bảo CreatedById lấy từ token

            var response = await _newsArticleService.CreateNewsArticle(newsArticle);

            if (response.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = response.Message;
            return View(newsArticle);
        }

    }

}

