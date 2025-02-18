using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult NewsManagementPage()
        {
            return View();
        }
    }
}
