using GroupBoizBLL.Services.Interface;
using GroupBoizBLL.Utilities;
using GroupBoizCommon.DTO;


using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
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
            var role = _userUtility.GetRoleFromToken();
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
        public async Task<IActionResult> UpdateNews([FromBody] NewsArticleDTO updatedNews)
        {
            if (updatedNews == null || string.IsNullOrEmpty(updatedNews.NewsArticleId))
            {
                return BadRequest(new { success = false, message = "Invalid news data." });
            }



            // Cập nhật bài viết trong cơ sở dữ liệu
            var updateResponse = await _newsArticleService.UpdateNewsArticle(updatedNews);

            if (updateResponse.IsSuccess)
            {
                return Ok(new { success = true, message = "News updated successfully!" });
            }


            return StatusCode(500, new { success = false, message = "Failed to update news." });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteNews([FromBody] string newsArticleId)
        {
            // Kiểm tra nếu newsArticleId là null hoặc rỗng
            Console.WriteLine(newsArticleId);
            if (string.IsNullOrEmpty(newsArticleId))
            {
                return BadRequest(new { success = false, message = "Invalid news ID." });
            }

            // Gọi service để xóa bài viết
            var deleteResponse = await _newsArticleService.DeleteNews(newsArticleId);

            // Kiểm tra phản hồi từ service
            if (deleteResponse.IsSuccess)
            {
                return Ok(new { success = true, message = "News deleted successfully!" });
            }

            return StatusCode(500, new { success = false, message = "Failed to delete news." });
        }
    }
    }
