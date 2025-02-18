using System.Diagnostics;
using GroupBoizBLL.Services.Implement;
using GroupBoizBLL.Services.Interface;
using GroupBoizMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        // Inject CategoryService vào controller thông qua constructor
        public HomeController(ICategoryService categoryService, ITagService tagService)
        {
            _categoryService = categoryService;
            _tagService = tagService;
        }

        // Action để hiển thị danh sách Category
        public async Task<IActionResult> Index()
        {
            // Gọi phương thức GetAll từ CategoryService để lấy dữ liệu
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();

            if (categoryResponse.IsSuccess && tagResponse.IsSuccess)
            {
                ViewBag.Category = categoryResponse.Result;  // Truyền categories vào view
                ViewBag.Tag = tagResponse.Result;  // Truyền tags vào view

                return View(); // Trả về view chính
            }
            else
            {
                ViewBag.ErrorMessage = categoryResponse.Message ?? tagResponse.Message;
                return View();
            }
        }
    }
}
