using GroupBoizDAL.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using GroupBoizDAL.Entities;
using System.Linq;

namespace GroupBoizMVC.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public IActionResult Index()
        {
            var newsList = _newsRepository.GetAll().ToList();
            return View(newsList);
        }

        // Xóa bài viết (Cập nhật NewsStatus thành 0)
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var news = _newsRepository.GetById(id);
            if (news != null)
            {
                news.NewsStatus = false; // Đánh dấu bài viết đã bị xóa
                _newsRepository.Update(news);
            }
            return RedirectToAction("Index");
        }

        // Lấy thông tin bài viết để hiển thị trong combobox khi chỉnh sửa
        [HttpGet]
        public JsonResult GetNewsById(string id)
        {
            var news = _newsRepository.GetById(id);
            if (news == null)
                return Json(null);

            return Json(news);
        }

        // Chỉnh sửa bài viết
        [HttpPost]
        public IActionResult Edit(NewsArticle updatedNews)
        {
            var existingNews = _newsRepository.GetById(updatedNews.NewsArticleId);
            if (existingNews != null)
            {
                existingNews.NewsTitle = updatedNews.NewsTitle;
                existingNews.NewsContent = updatedNews.NewsContent;
                existingNews.NewsSource = updatedNews.NewsSource;
                existingNews.ModifiedDate = DateTime.Now;

                _newsRepository.Update(existingNews);
            }
            return RedirectToAction("Index");
        }
    }
}
