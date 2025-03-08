using GroupBoizBLL.Hubs;
using GroupBoizBLL.Services.Interface;
using GroupBoizBLL.Utilities;
using GroupBoizCommon.DTO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GroupBoizMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserUtility _userUtility;
        private readonly IHubContext<AllHub> _hubContext;

        public ProfileController(IAccountService accountService, UserUtility userUtility, IHubContext<AllHub> hubContext)
        {
            _accountService = accountService;
            _userUtility = userUtility;
            _hubContext = hubContext;
        }

        // Hiển thị thông tin profile
        public async Task<IActionResult> Index()
        {
            
            short userId = _userUtility.GetUserIDFromToken();
            var response = await _accountService.GetById(userId);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            ViewBag.ErrorMessage = response.Message;
            return View(new SystemAccountDTO());
        }

        // Cập nhật thông tin profile
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] SystemAccountDTO account)
        {
            if (account == null || account.AccountId == 0)
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            var response = await _accountService.UpdateAccountAsync(account);

            if (response.IsSuccess)
            {

                await _hubContext.Clients.All.SendAsync("ReloadPage");

                return Json(new { success = true, message = "Profile updated successfully!" });
            }

            return Json(new { success = false, message = response.Message });
        }
    }
}
