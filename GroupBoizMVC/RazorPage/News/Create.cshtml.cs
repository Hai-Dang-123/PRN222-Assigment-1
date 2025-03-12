using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using GroupBoizBLL.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupBoizBLL.Hubs;

namespace GroupBoizMVC.Pages.News
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly INewsArticleService _newsArticleService;
        private readonly UserUtility _userUtility;
        private readonly IHubContext<AllHub> _hubContext; // 🔥 Inject SignalR Hub

        public CreateModel(
            ICategoryService categoryService,
            ITagService tagService,
            INewsArticleService newsArticleService,
            UserUtility userUtility,
            IHubContext<AllHub> hubContext) // ✅ Nhận SignalR Hub từ DI
        {
            _categoryService = categoryService;
            _tagService = tagService;
            _newsArticleService = newsArticleService;
            _userUtility = userUtility;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticleDTO News { get; set; } = new NewsArticleDTO();


        [BindProperty]
        public List<int> SelectedTags { get; set; } = new();

        public List<CategoryDTO> Categories { get; set; }
        public List<TagDTO> Tags { get; set; }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();

            if (categoryResponse.IsSuccess && tagResponse.IsSuccess)
            {
                Categories = (List<CategoryDTO>)categoryResponse.Result;
                Tags = (List<TagDTO>)tagResponse.Result;
            }
            else
            {
                ErrorMessage = "Failed to load categories or tags.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Console.WriteLine("Selected Tags: " + string.Join(", ", SelectedTags ?? new List<int>()));

            //// Gán danh sách tag
            News.TagId = SelectedTags ?? new List<int>(); // Đảm bảo không bị null


            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Field: {state.Key}, Error: {error.ErrorMessage}");
                }
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

                ErrorMessage = "Invalid data! Errors: " + string.Join(", ", errors);

                return await OnGet(); // Load lại trang với thông báo lỗi
            }

            News.CreatedById = _userUtility.GetUserIDFromToken();

            

            var response = await _newsArticleService.CreateNewsArticle(News);
            if (response.IsSuccess)
            {
                SuccessMessage = "News article created successfully!";

                // 🔥 Gửi tín hiệu SignalR để tất cả client reload trang
                await _hubContext.Clients.All.SendAsync("ReloadPage");

                return RedirectToPage("/News/Index");
            }
            else
            {
                ErrorMessage = response.Message ?? "Failed to create news.";
                return await OnGet();
            }
        }
    }
}
