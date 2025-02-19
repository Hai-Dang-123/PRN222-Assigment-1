using GroupBoizBLL.Services.Interface;
using GroupBoizDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly INewsArticleService _newsArticleService;

        public DashboardController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }


        // GET: DashboardController
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var response = await _newsArticleService.GetNewsByPeriod(startDate, endDate);

            if (!response.IsSuccess)
            {
                ViewData["ErrorMessage"] = response.Message;
                return View(new List<NewsArticle>()); // Trả về danh sách trống
            }

            return View(response.Result);
        }

        //// GET: DashboardController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: DashboardController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: DashboardController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: DashboardController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: DashboardController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: DashboardController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: DashboardController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
