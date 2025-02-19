using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    public class AuthController : Controller
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Login()
        {
            
            return View();
        }

        public IActionResult Logout()
        {
            // Xóa session hoặc cookie đăng nhập
            return RedirectToAction("Login");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.Login(loginDTO);

                if (response.IsSuccess)
                {
                    var result = response.Result as dynamic;
                    // Save access token and refresh token (cookie or session)
                    TempData["AccessToken"] = result?.AccessToken;
                    TempData["RefreshToken"] = result?.RefreshToken;

                    return RedirectToAction("Index", "Home"); // Redirect to Home page after successful login
                }
                else
                {
                    ModelState.AddModelError("", response.Message); // Show error message
                }
            }

            return View(loginDTO); // Return to login page with error message if login fails
        }
    }
}
