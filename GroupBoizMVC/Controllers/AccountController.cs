using GroupBoizBLL.Services.Interface;
using GroupBoizDAL.Entities;
using GroupBoizDAL.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return View(accounts); // Trả về danh sách tài khoản cho View
        }


    }
}
