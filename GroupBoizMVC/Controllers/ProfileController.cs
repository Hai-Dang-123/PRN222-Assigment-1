using GroupBoizBLL.Services.Interface;
using GroupBoizBLL.Utilities;
using GroupBoizDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GroupBoizMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserUtility _userUtility;

        public ProfileController(IAccountService accountService, UserUtility userUtility)
        {
            _accountService = accountService;
            _userUtility = userUtility;
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
            return View(new SystemAccount());
        }

        // Cập nhật thông tin profile
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] SystemAccount account)
        {
            if (account == null || account.AccountId == 0)
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            var response = await _accountService.UpdateAccount(account);

            if (response.IsSuccess)
            {
                return Json(new { success = true, message = "Profile updated successfully!" });
            }

            return Json(new { success = false, message = response.Message });
        }
    }
}
