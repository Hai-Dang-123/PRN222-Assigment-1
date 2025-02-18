using GroupBoizBLL.Services.Interface;
using GroupBoizDAL.Entities;
using GroupBoizDAL.Repository.Implement;
using GroupBoizDAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBoizBLL.Services.Implement
{
    public  class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<SystemAccount> CreateAccountAsync(SystemAccount account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            // Kiểm tra email đã tồn tại chưa
            bool emailExists = await _accountRepository.FindByEmailAsync(account.AccountEmail) != null;
            if (emailExists)
            {
                throw new InvalidOperationException("Email đã tồn tại.");
            }

            return await _accountRepository.CreateAccountAsync(account); // Lưu tài khoản vào cơ sở dữ liệu
        }


        public async Task<SystemAccount> UpdateAccountAsync(short accountId, SystemAccount updatedAccount)
        {
            var existingAccount = await _accountRepository.FindByIdAsync(accountId);
            if (existingAccount == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }

            existingAccount.AccountName = updatedAccount.AccountName;
            existingAccount.AccountEmail = updatedAccount.AccountEmail;
            existingAccount.AccountRole = updatedAccount.AccountRole;
            existingAccount.AccountPassword = updatedAccount.AccountPassword;

            await _accountRepository.UpdateAccountAsync(existingAccount);
            return existingAccount;
        }
        public async Task<IEnumerable<SystemAccount>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

    }
}