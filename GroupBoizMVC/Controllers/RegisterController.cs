using GroupBoizCommon.DTO;
using GroupBoizBLL.Services;
using Microsoft.AspNetCore.Mvc;
using GroupBoizBLL.Services.Interface;

namespace GroupBoizMVC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IAuthService _authService;

        public RegisterController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDTO);
            }

            var response = await _authService.Register(registerDTO);

            if (!response.IsSuccess)
            {
                ViewBag.ErrorMessage = response.Message;
                return View(registerDTO);
            }

            // 🟢 Lưu message vào TempData trước khi redirect
            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";

            return RedirectToAction("Index", "Login");

            return RedirectToAction("Index", "Login");
        }
    }
}
