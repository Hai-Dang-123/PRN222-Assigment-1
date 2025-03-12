using GroupBoizBLL.Hubs;
using GroupBoizBLL.Services.Interface;
using GroupBoizBLL.Utilities;
using GroupBoizCommon.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace GroupBoizMVC.RazorPage.Profile
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly UserUtility _userUtility;
        private readonly IHubContext<AllHub> _hubContext;

        public IndexModel(IAccountService accountService, UserUtility userUtility, IHubContext<AllHub> hubContext)
        {
            _accountService = accountService;
            _userUtility = userUtility;
            _hubContext = hubContext;
        }
        [BindProperty]
        public SystemAccountDTO Account { get; set; } = new SystemAccountDTO();

        public async Task<IActionResult> OnGetAsync()
        {
            short userId = _userUtility.GetUserIDFromToken();
            var response = await _accountService.GetById(userId);

            if (!response.IsSuccess || response.Result == null)
            {
                return RedirectToPage("/Error");
            }

            Account = response.Result as SystemAccountDTO ?? new SystemAccountDTO();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (Account == null || Account.AccountId == 0)
            {
                return BadRequest(new { success = false, message = "Invalid data!" });
            }

            var response = await _accountService.UpdateAccountAsync(Account);

            if (response.IsSuccess)
            {
                await _hubContext.Clients.All.SendAsync("ReloadPage");
                return new JsonResult(new { success = true, message = "Profile updated successfully!" });
            }

            return new JsonResult(new { success = false, message = response.Message });
        }
    }
}
