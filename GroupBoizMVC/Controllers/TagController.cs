using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    public class TagController : Controller
    {
        public IActionResult TagManagementPage()
        {
            return View();
        }
    }
}
