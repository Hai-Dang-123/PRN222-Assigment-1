using GroupBoizBLL.Services.Interface;
using GroupBoizDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly IAccountService _accountService;
        public AccountController (IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return View(accounts); // Trả về danh sách tài khoản cho View   
        }

        [HttpPost]
        public async Task<IActionResult> Edit(short id, [FromBody] SystemAccount updatedAccount)
        {
            if (updatedAccount == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                Console.WriteLine($"Trước khi cập nhật: ID = {id}, Name = {updatedAccount.AccountName}, Email = {updatedAccount.AccountEmail}");

                var existingAccount = await _accountService.UpdateAccountAsync(id, updatedAccount);

                if (existingAccount == null)
                {
                    return NotFound("Tài khoản không tồn tại.");
                }

                Console.WriteLine($"Sau khi cập nhật: ID = {id}, Name = {existingAccount.AccountName}, Email = {existingAccount.AccountEmail}");

                return Ok(new { message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }
        [HttpDelete("deleteaccount/{id}")]
        public async Task<IActionResult> DeleteAccount(short id)
        {
            try
            {
                var isDeleted = await _accountService.DeleteAccountAsync(id);
                if (!isDeleted)
                {
                    return NotFound("Tài khoản không tồn tại.");
                }
                return Ok(new { message = "Xóa tài khoản thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }





    }
}
