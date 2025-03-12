using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GroupBoizMVC.RazorPage.Search
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public IndexModel(INewsArticleService newsService, ICategoryService categoryService, ITagService tagService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
            _tagService = tagService;
        }
        [BindProperty(SupportsGet = true)]
        public string? Query { get; set; }

        public List<NewsArticleDTO> NewsArticles { get; set; } = new();
        public List<CategoryDTO> Categories { get; set; } = new();
        public List<TagDTO> Tags { get; set; } = new();
        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy danh sách danh mục và tags
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();
            Categories = categoryResponse.Result as List<CategoryDTO> ?? new List<CategoryDTO>();
            Tags = tagResponse.Result as List<TagDTO> ?? new List<TagDTO>();

            if (string.IsNullOrWhiteSpace(Query))
            {
                NewsArticles = new List<NewsArticleDTO>(); // Trả về danh sách rỗng nếu không có từ khóa
                return Page();
            }

            // Gọi service để tìm kiếm bài viết theo tiêu đề
            var searchResults = await _newsService.SearchNewsByTitle(Query);
            NewsArticles = searchResults.Result as List<NewsArticleDTO> ?? new List<NewsArticleDTO>();

            return Page();
        }
    }
}
