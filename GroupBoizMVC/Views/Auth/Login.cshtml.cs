using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GroupBoizMVC.Views.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public LoginDTO LoginDTO { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.Login(LoginDTO);

                if (response.IsSuccess)
                {
                    var result = response.Result as dynamic;
                    // Save access token and refresh token (session or cookie)
                    TempData["AccessToken"] = result?.AccessToken;
                    TempData["RefreshToken"] = result?.RefreshToken;

                    return RedirectToPage("/Index"); // Redirect to Home page after successful login
                }
                else
                {
                    ErrorMessage = response.Message; // Set error message
                }
            }

            return Page(); // Return to the login page with error message
        }
    }
}
