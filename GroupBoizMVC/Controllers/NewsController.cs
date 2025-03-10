using Azure;
using GroupBoizBLL.Hubs;
using GroupBoizBLL.Services.Interface;
using GroupBoizBLL.Utilities;
using GroupBoizCommon.DTO;


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace GroupBoizMVC.Controllers
{
    public class NewsController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly INewsArticleService _newsArticleService;
        private readonly UserUtility _userUtility;
        private readonly IHubContext<AllHub> _hubContext; // 🔥 Inject SignalR Hub
        public NewsController (ICategoryService categoryService, ITagService tagService,  INewsArticleService newsArticleService, UserUtility userUtility, IHubContext<AllHub> hubContext)
        {
            _categoryService = categoryService;
            _tagService = tagService;
            _newsArticleService = newsArticleService;
            _userUtility = userUtility;
            _hubContext = hubContext;
        }

        // Action để hiển thị danh sách Category
        public async Task<IActionResult> Index()
        {
            // Gọi phương thức GetAll từ CategoryService để lấy dữ liệu
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();
            var newsForStaff = await _newsArticleService.GetAllNewsWithTag();
            var newsForLecturer = await _newsArticleService.GetAllNewsActiveWithTag();

            var role = _userUtility.GetRoleFromToken();  // Lấy role từ token
            var userId = _userUtility.GetUserIDFromToken();  // Lấy user ID từ token

            Console.WriteLine(userId);
            Console.WriteLine(role);

            // Kiểm tra các phản hồi từ các service
            if (categoryResponse.IsSuccess && tagResponse.IsSuccess && newsForLecturer.IsSuccess && newsForStaff.IsSuccess)
            {
                ViewBag.Category = categoryResponse.Result;  // Truyền categories vào view
                ViewBag.Tag = tagResponse.Result;  // Truyền tags vào view
                ViewBag.Role = role;

                // Tùy thuộc vào vai trò của người dùng, phân loại tin tức
                if (role == "Staff")
                {
                    ViewBag.News = newsForStaff.Result;  // Dành cho Staff
                }
                else if (role == "Lecturer")
                {
                    ViewBag.News = newsForLecturer.Result;  // Dành cho Lecturer
                }
                else
                {
                    ViewBag.News = new List<NewsArticleDTO>();  // Tránh null nếu không phải Staff hoặc Lecturer
                }

                return View();  // Trả về view chính
            }
            else
            {
                // Truyền thông báo lỗi nếu có bất kỳ response nào không thành công
                ViewBag.ErrorMessage = categoryResponse.Message ?? tagResponse.Message ?? "An error occurred.";
                ViewBag.TopThreeNews = new List<NewsArticleDTO>();  // Tránh null reference
                return View();
            }
        }

        // 📌 ByCategory - Lọc bài viết theo danh mục
        public async Task<IActionResult> ByCategory(int id)
        {
            var articles = await _newsArticleService.GetByCategoryAsync(id);
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();
            var role = _userUtility.GetRoleFromToken();

            ViewBag.Role = role;
            ViewBag.News = articles.Result;
            ViewBag.Category = categoryResponse.Result;
            ViewBag.Tag = tagResponse.Result;

            

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
            ViewBag.Category = categoryResponse.Result;
            ViewBag.Tag = tagResponse.Result;

           

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
                ViewBag.SuccessMessage = response.Message;
                // 🔥 Khi bài viết mới được tạo, gửi tín hiệu cho tất cả client reload trang
                await _hubContext.Clients.All.SendAsync("ReloadPage");

                
            }

            ViewBag.ErrorMessage = response.Message;
            return View(newsArticle);
        }



    }

}

