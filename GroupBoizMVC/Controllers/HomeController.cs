using System.Diagnostics;
using System.Security.Claims;
using Azure;
using GroupBoizBLL.Services.Implement;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;

using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly INewsArticleService _newsArticleService;

        // Inject CategoryService vào controller thông qua constructor
        public HomeController(ICategoryService categoryService, ITagService tagService, INewsArticleService newsArticleService)
        {
            _categoryService = categoryService;
            _tagService = tagService;
            _newsArticleService = newsArticleService;
        }

        // Action để hiển thị danh sách Category
        public async Task<IActionResult> Index()
        {
            // Gọi phương thức GetAll từ CategoryService để lấy dữ liệu
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();
            var newsResponse = await _newsArticleService.GetAllNewsWithTag();
            //var userRole = User.FindFirstValue(ClaimTypes.Role); // Lấy role từ Claims

            if (categoryResponse.IsSuccess && tagResponse.IsSuccess && newsResponse.IsSuccess )
            {
                ViewBag.Category = categoryResponse.Result;  // Truyền categories vào view
                ViewBag.Tag = tagResponse.Result;  // Truyền tags vào view
                ViewBag.News = newsResponse.Result;
                //ViewBag.UserRole = userRole;

                return View(); // Trả về view chính
            }
            else
            {
                ViewBag.ErrorMessage = categoryResponse.Message ?? tagResponse.Message;
                ViewBag.TopThreeNews = new List<GroupBoizCommon.DTO.NewsArticleDTO>();  // Tránh null reference
                return View();
            }
        }
    }
}
