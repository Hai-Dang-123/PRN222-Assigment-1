using GroupBoizBLL.Hubs;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace GroupBoizMVC.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IHubContext<AllHub> _hubContext;

        public IndexModel(IAccountService accountService, IHubContext<AllHub> hubContext)
        {
            _accountService = accountService;
            _hubContext = hubContext;
        }

        public List<SystemAccountDTO> Accounts { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _accountService.GetAllAccountsAsync();
            if (response.IsSuccess)
            {
                Accounts = (List<SystemAccountDTO>)response.Result;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(short id, [FromBody] SystemAccountDTO updatedAccount)
        {
            if (updatedAccount == null)
            {
                return BadRequest("Invalid data.");
            }

            updatedAccount.AccountId = id;
            var response = await _accountService.UpdateAccountAsync(updatedAccount);

            if (!response.IsSuccess)
            {
                return NotFound(response.Message);
            }

            return new JsonResult(new { message = "Cập nhật thành công!", data = response.Result });
        }

        public async Task<IActionResult> OnPostDeleteAccountAsync(short id)
        {
            var response = await _accountService.DeleteAccountAsync(id);
            if (!response.IsSuccess)
            {
                return NotFound(response.Message);
            }
            return new JsonResult(new { message = response.Message });
        }

        public async Task<IActionResult> OnPostToggleAccountStatusAsync([FromBody] ToggleAccountStatusDTO request)
        {
            var result = await _accountService.ToggleAccountStatusAsync(request.AccountId, request.IsEnable);

            if (!result)
                return NotFound(new { message = "User not found" });

            if (!request.IsEnable)
            {
                await _hubContext.Clients.Group(request.AccountId.ToString()).SendAsync("AccountBlocked");
            }

            return new JsonResult(new { message = "Account status updated successfully" });
        }
    }
}
