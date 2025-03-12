using System.Net.WebSockets;
using System.Text.Json;
using GroupBoizBLL.Hubs;
using GroupBoizBLL.Services.Interface;
using GroupBoizBLL.Utilities;
using GroupBoizCommon.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

using System.Text.Json;

namespace GroupBoizMVC.RazorPage.NewsDetail
{
    public class IndexModel : PageModel
    {

        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly INewsArticleService _newsArticleService;
        private readonly UserUtility _userUtility;
        private readonly IHubContext<AllHub> _hubContext;

        public IndexModel(ICategoryService categoryService, ITagService tagService, INewsArticleService newsArticleService, UserUtility userUtility, IHubContext<AllHub> hubContext)
        {
            _categoryService = categoryService;
            _tagService = tagService;
            _newsArticleService = newsArticleService;
            _userUtility = userUtility;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticleDTO News { get; set; }

        public List<TagDTO> AllTags { get; set; } = new List<TagDTO>();
        public List<CategoryDTO> AllCategories { get; set; } = new List<CategoryDTO>();
        public string Role { get; set; }
        public short AccountId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Console.WriteLine("lấy được id rồi nhé ",id);
            var response = await _newsArticleService.GetNewsById(id);
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();

            Role = _userUtility.GetRoleFromToken();
            AccountId = _userUtility.GetUserIDFromToken();

            if (!response.IsSuccess || response.Result == null)
            {
                return RedirectToPage("/News/NotFoundPage");
            }
            Console.WriteLine($"🛠 response.Result Type: {response.Result?.GetType()}");

            //var newsResult = response.Result as NewsArticleDTO ?? new NewsArticleDTO();
            // ⚠️ Ép kiểu tường minh để tránh lỗi
            News = response.Result as NewsArticleDTO ?? new NewsArticleDTO(); ;
            // 🛠 Chuyển kiểu chính xác để tránh lỗi null
            //var json = JsonConvert.SerializeObject(response.Result);
            //News = JsonConvert.DeserializeObject<NewsArticleDTO>(json) ?? new NewsArticleDTO();
            Console.WriteLine($"✅ News Loaded: {News.NewsTitle ?? "No title"}");
            AllTags = tagResponse.Result as List<TagDTO> ?? new List<TagDTO>();
            AllCategories = categoryResponse.Result as List<CategoryDTO> ?? new List<CategoryDTO>();

            return Page();
        }

       
        public async Task<IActionResult> OnPostUpdateNewsAsync(NewsArticleDTO updatedNews)
        {
            if (updatedNews == null)
            {
                Console.WriteLine("❌ Received NULL updatedNews");
                return BadRequest(new { success = false, message = "Invalid news data." });
            }

            Console.WriteLine($"📥 Received Update Request: {JsonSerializer.Serialize(updatedNews)}");

            if (string.IsNullOrEmpty(updatedNews.NewsArticleId))
            {
                Console.WriteLine("❌ NewsArticleId is missing!");
                return BadRequest(new { success = false, message = "NewsArticleId is required." });
            }

            var updateResponse = await _newsArticleService.UpdateNewsArticle(updatedNews);

            if (updateResponse.IsSuccess)
            {
                Console.WriteLine("✅ Update successful!");
                return new JsonResult(new { success = true, message = "News updated successfully!" });
            }

            Console.WriteLine("❌ Update failed!");
            return StatusCode(500, new { success = false, message = "Failed to update news." });
        }


        public async Task<IActionResult> OnPostDeleteNewsAsync([FromBody] string newsArticleId)
        {
            if (string.IsNullOrEmpty(newsArticleId))
            {
                return BadRequest(new { success = false, message = "Invalid news ID." });
            }

            var deleteResponse = await _newsArticleService.DeleteNews(newsArticleId);

            if (deleteResponse.IsSuccess)
            {
                await _hubContext.Clients.All.SendAsync("ReloadPage");
                return new JsonResult(new { success = true, message = "News deleted successfully!" });
            }

            return StatusCode(500, new { success = false, message = "Failed to delete news." });
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync([FromBody] ArticleStatusDTO request)
        {
            var response = await _newsArticleService.UpdateStatus(request.ArticleId, request.Status);

            if (response.IsSuccess)
            {
                return new JsonResult(new { success = true, message = "Status updated successfully!" });
            }
            else
            {
                return new JsonResult(new { success = false, message = response.Message });
            }
        }
    }
}
