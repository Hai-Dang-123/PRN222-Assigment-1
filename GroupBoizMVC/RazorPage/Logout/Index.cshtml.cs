using GroupBoizBLL.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GroupBoizMVC.RazorPage.Logout
{
    public class IndexModel : PageModel
    {
        private readonly IAuthService _authService;

        public IndexModel(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<IActionResult> Index()
        {
            await _authService.LogoutAsync(); // Gọi hàm logout từ service

            return RedirectToAction("Index", "Home"); // Chuyển hướng về trang login
        }
    }
}
