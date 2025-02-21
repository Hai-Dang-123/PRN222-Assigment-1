using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using GroupBoizBLL.Services.Interface; // Import service nếu cần

namespace GroupBoizMVC.Controllers
{
    public class LogoutController : Controller
    {
        private readonly IAuthService _authService;

        public LogoutController(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            await _authService.LogoutAsync(); // Gọi hàm logout từ service

            return RedirectToAction("Index", "Login"); // Chuyển hướng về trang login
        }
    }
}
