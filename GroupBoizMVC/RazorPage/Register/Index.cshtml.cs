using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GroupBoizMVC.RazorPage.Register
{
    public class IndexModel : PageModel
    {
        private readonly IAuthService _authService;

        public IndexModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public RegisterDTO Register { get; set; } = new RegisterDTO();

        [TempData]
        public string? SuccessMessage { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _authService.Register(Register);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                return Page();
            }

            // 🟢 Lưu message vào TempData trước khi redirect
            SuccessMessage = "Đăng ký thành công! Vui lòng đăng nhập.";

            return RedirectToPage("/Login/Index");
        }
    }
}
