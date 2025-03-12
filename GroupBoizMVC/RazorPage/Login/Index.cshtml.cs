using System;
using System.Threading.Tasks;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GroupBoizMVC.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly IAuthService _authService;

        [BindProperty]
        public LoginDTO LoginModel { get; set; } = new LoginDTO();

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public IndexModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
            if (TempData["SuccessMessage"] != null)
            {
                SuccessMessage = TempData["SuccessMessage"].ToString();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Thông tin đăng nhập không hợp lệ!";
                return Page();
            }

            var loginResponse = await _authService.Login(LoginModel);

            if (loginResponse.IsSuccess)
            {
                var token = loginResponse.Result as TokenDTO;
                if (token != null)
                {
                    Response.Cookies.Append("AccessToken", token.AccessToken, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddMinutes(15)
                    });

                    Response.Cookies.Append("RefreshToken", token.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });

                    return RedirectToPage("/Home/Index");
                }
            }

            ErrorMessage = loginResponse.Message;
            return Page();
        }
    }
}
