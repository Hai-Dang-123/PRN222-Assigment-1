using Microsoft.AspNetCore.Mvc;

using GroupBoizBLL.Services.Interface;  // Thêm namespace cho service
using System.Threading.Tasks;
using GroupBoizCommon.DTO;
using Azure;

namespace GroupBoizMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthService _authService; // Dịch vụ xác thực (IAuthService)

        // Constructor injection service
        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View(new LoginDTO());
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Index(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Thông tin đăng nhập không hợp lệ!";
                return View(loginDTO); // Trả lại trang đăng nhập nếu dữ liệu không hợp lệ
            }

            // Gọi phương thức Login từ service
            var loginResponse = await _authService.Login(loginDTO);

            if (loginResponse.IsSuccess)
            {
                // Debug để kiểm tra kiểu thực tế của Result
                Console.WriteLine($"Result Type: {loginResponse.Result?.GetType()}");
                var token = loginResponse.Result as TokenDTO;
                if (token != null)
                {
                    var accessToken = token.AccessToken;
                    var refreshToken = token.RefreshToken;
                    // Lưu accessToken và refreshToken vào cookie nếu login thành công
                    Response.Cookies.Append("AccessToken", accessToken, new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddMinutes(15)
                    });

                    Response.Cookies.Append("RefreshToken", refreshToken, new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });

                    return RedirectToAction("Index", "Home");
                }
            }

            // Nếu đăng nhập thất bại, sử dụng ViewBag để truyền thông báo lỗi
            ViewBag.Message = loginResponse.Message;
            return View(loginDTO);
        }

    }
}

