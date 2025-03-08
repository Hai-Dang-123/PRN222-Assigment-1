using GroupBoizBLL.Hubs;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GroupBoizMVC.Controllers
{

    public class AccountController : Controller
    {

        private readonly IAccountService _accountService;
        private readonly IHubContext<AllHub> _hubContext; // 🔥 Inject SignalR Hub
        public AccountController(IAccountService accountService , IHubContext<AllHub> hubContext)
        {
            _accountService = accountService;
            _hubContext = hubContext;
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

                var updatedData = response.Result;




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


        [HttpPost()]
        public async Task<IActionResult> ToggleAccountStatus([FromBody] ToggleAccountStatusDTO request)
        {
            // Debug dữ liệu nhận được
            Console.WriteLine($"📩 Received: AccountId = {request.AccountId}, IsEnable = {request.IsEnable}");

            var result = await _accountService.ToggleAccountStatusAsync(request.AccountId, request.IsEnable);

            if (!result)
                return NotFound(new { message = "User not found" });


            // 🛑 Nếu bị block, gửi tín hiệu yêu cầu logout
            if (!request.IsEnable)
            {
                Console.WriteLine($"🔴 [DEBUG] Đang gửi sự kiện AccountBlocked tới UserID: {request.AccountId}");
                await _hubContext.Clients.Group(request.AccountId.ToString()).SendAsync("AccountBlocked");


                Console.WriteLine("✅ [DEBUG] Sự kiện AccountBlocked đã được gửi đi!");

            }

            return Ok(new { message = "Account status updated successfully" });
        }


    }
}