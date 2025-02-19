using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace GroupBoizMVC.Controllers
{
    public class AuthController : Controller
    {
        // GET: AuthController
        public ActionResult Index()
        {
            return View();
        }

        // ✅ API kiểm tra AccessToken
        [HttpGet]
        public IActionResult CheckToken()
        {
            var accessToken = Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("❌ Token không tồn tại.");

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(accessToken);
                var exp = token.ValidTo; // Lấy thời gian hết hạn của token

                Console.WriteLine($"🔍 Kiểm tra token - Hết hạn vào: {exp}");

                if (exp < DateTime.UtcNow)
                    return Unauthorized("⚠️ Token đã hết hạn.");

                return Ok("✅ Token hợp lệ.");
            }
            catch (Exception ex)
            {
                return Unauthorized($"❌ Lỗi kiểm tra token: {ex.Message}");
            }
        }
    }
}
