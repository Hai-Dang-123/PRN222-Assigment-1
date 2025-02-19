using GroupBoizBLL.Services.Interface;
using GroupBoizBLL.Utilities;
using GroupBoizDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace GroupBoizMVC.Controllers
{
    public class NewsDetailController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly INewsArticleService _newsArticleService;
        private readonly UserUtility _userUtility;

        public NewsDetailController(ICategoryService categoryService, ITagService tagService, INewsArticleService newsArticleService, UserUtility userUtility)
        {
            _categoryService = categoryService;
            _tagService = tagService;
            _newsArticleService = newsArticleService;
            _userUtility = userUtility;
        }

        public async Task<IActionResult> Index(string id)
        {
            var response = await _newsArticleService.GetNewsById(id);
            var categoryResponse = await _categoryService.GetAll();
            var tagResponse = await _tagService.GetAllTags();
            var role =  _userUtility.GetRoleFromToken();
            var accountId = _userUtility.GetUserIDFromToken();

            if (!response.IsSuccess)
            {
                return RedirectToAction("NotFoundPage", "News");
            }

            ViewBag.News = response.Result;
            ViewBag.Tag = tagResponse.Result;
            ViewBag.Category = categoryResponse.Result;
            ViewBag.Role = role;
            ViewBag.AccountId = accountId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNews([FromBody] NewsArticle updatedNews)
        {
            if (updatedNews == null || string.IsNullOrEmpty(updatedNews.NewsArticleId))
            {
                return BadRequest(new { success = false, message = "Invalid news data." });
            }

            var existingNews = await _newsArticleService.GetNewsById(updatedNews.NewsArticleId) ;
            if (!existingNews.IsSuccess)
            {
                return NotFound(new { success = false, message = "News not found." });
            }

            var newsToUpdate = existingNews.Result as NewsArticle;

            newsToUpdate.NewsTitle = updatedNews.NewsTitle;
            newsToUpdate.NewsContent = updatedNews.NewsContent;

            var updateResponse = await _newsArticleService.UpdateNewsArticle(newsToUpdate);

            if (updateResponse.IsSuccess)
            {
                return Ok(new { success = true, message = "News updated successfully!" });
            }

            return StatusCode(500, new { success = false, message = "Failed to update news." });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNews([FromBody] NewsArticle news)
        {
            if (news == null || string.IsNullOrEmpty(news.NewsArticleId))
            {
                return BadRequest(new { success = false, message = "Invalid news ID." });
            }

            var deleteResponse = await _newsArticleService.DeleteNews(news.NewsArticleId);

            if (deleteResponse.IsSuccess)
            {
                return Ok(new { success = true, message = "News deleted successfully!" });
            }

            return StatusCode(500, new { success = false, message = "Failed to delete news." });
        }
    }
}
