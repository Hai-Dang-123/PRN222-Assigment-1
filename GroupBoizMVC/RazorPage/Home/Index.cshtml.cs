using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GroupBoizBLL.Services.Interface;
using GroupBoizBLL.Utilities;
using GroupBoizCommon.DTO;

namespace GroupBoizMVC.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly INewsArticleService _newsArticleService;
        private readonly UserUtility _userUtility;

        public List<CategoryDTO> Categories { get; set; } = new();
        public List<TagDTO> Tags { get; set; } = new();
        public List<NewsArticleDTO> NewsArticles { get; set; } = new();
        public string UserRole { get; set; } = string.Empty;

        public IndexModel(ICategoryService categoryService, ITagService tagService,
                          INewsArticleService newsArticleService, UserUtility userUtility)
        {
            _categoryService = categoryService;
            _tagService = tagService;
            _newsArticleService = newsArticleService;
            _userUtility = userUtility;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();
            var newsResponse = await _newsArticleService.GetAllNewsActiveWithTag();
            UserRole = _userUtility.GetRoleFromToken();

            if (categoryResponse.IsSuccess && tagResponse.IsSuccess && newsResponse.IsSuccess)
            {
                Categories = categoryResponse.Result as List<CategoryDTO> ?? new List<CategoryDTO>();
                Tags = tagResponse.Result as List<TagDTO> ?? new List<TagDTO>();
                NewsArticles = newsResponse.Result as List<NewsArticleDTO> ?? new List<NewsArticleDTO>();

            }

            return Page();
        }
    }
}
