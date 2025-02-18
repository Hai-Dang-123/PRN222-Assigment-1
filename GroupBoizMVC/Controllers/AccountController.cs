using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
        

    }
}
