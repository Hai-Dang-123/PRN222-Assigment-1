using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
 
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
            return View(accounts.Result); // Trả về danh sách tài khoản cho View   
        }

        [HttpPost]
        public async Task<IActionResult> Edit(short id, [FromBody] SystemAccountDTO updatedAccount)
        {
            if (updatedAccount == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                Console.WriteLine($"Trước khi cập nhật: ID = {updatedAccount.AccountId}, Name = {updatedAccount.AccountName}, Email = {updatedAccount.AccountEmail}, Role = {updatedAccount.AccountRole}");
                updatedAccount.AccountId = id;
               
                var response = await _accountService.UpdateAccountAsync(updatedAccount);

                if (!response.IsSuccess)
                {
                    return NotFound(response.Message);
                }

                var updatedData = response.Result ;


               

                return Ok(new { message = "Cập nhật thành công!", data = updatedData });
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
                var response = await _accountService.DeleteAccountAsync(id);
                if (!response.IsSuccess)
                {
                    return NotFound(response.Message);
                }
                return Ok(new { message = response.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }





    }
}
