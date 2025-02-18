using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult CategoryManagementPage()
        {
            return View();
        }
    }
}
