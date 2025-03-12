using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using GroupBoizBLL.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupBoizMVC.RazorPage.News
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly INewsArticleService _newsArticleService;
        private readonly UserUtility _userUtility;

        public IndexModel(ICategoryService categoryService, ITagService tagService, INewsArticleService newsArticleService, UserUtility userUtility)
        {
            _categoryService = categoryService;
            _tagService = tagService;
            _newsArticleService = newsArticleService;
            _userUtility = userUtility;
        }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string Role { get; set; }
        public List<CategoryDTO> Categories { get; set; }
        public List<TagDTO> Tags { get; set; }
        public List<NewsArticleDTO> News { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();
            var newsForStaff = await _newsArticleService.GetAllNewsWithTag();
            var newsForLecturer = await _newsArticleService.GetAllNewsActiveWithTag();

            Role = _userUtility.GetRoleFromToken();

            if (categoryResponse.IsSuccess && tagResponse.IsSuccess && newsForLecturer.IsSuccess && newsForStaff.IsSuccess)
            {
                Categories = categoryResponse.Result as List<CategoryDTO> ?? new List<CategoryDTO>();
                Tags = tagResponse.Result as List<TagDTO> ?? new List<TagDTO>();

                News = Role == "Staff"
                    ? newsForStaff.Result as List<NewsArticleDTO> ?? new List<NewsArticleDTO>()
                    : Role == "Lecturer"
                    ? newsForLecturer.Result as List<NewsArticleDTO> ?? new List<NewsArticleDTO>()
                    : new List<NewsArticleDTO>();

            }
            else
            {
                ErrorMessage = "An error occurred while loading data.";
            }

            return Page();
        }
    }
}
