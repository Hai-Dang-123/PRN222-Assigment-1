using GroupBoizBLL.Services.Interface;
using GroupBoizDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GroupBoizMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAccountService _accountService;

        public ProfileController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Hiển thị thông tin profile
        public async Task<IActionResult> Index()
        {
            short userId = 1; // Giả lập ID người dùng (Sau này lấy từ session)
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
