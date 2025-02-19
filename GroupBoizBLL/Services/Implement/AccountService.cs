using GroupBoizBLL.Services.Interface;
using GroupBoizDAL.Entities;
using GroupBoizDAL.Repository.Implement;
using GroupBoizDAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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




        public async Task<SystemAccount> UpdateAccountAsync(short accountId, SystemAccount updatedAccount)
        {
            var existingAccount = await _accountRepository.FindByIdAsync(accountId);
            if (existingAccount == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }

            if (!string.IsNullOrEmpty(updatedAccount.AccountName))
                existingAccount.AccountName = updatedAccount.AccountName;

            if (!string.IsNullOrEmpty(updatedAccount.AccountEmail))
                existingAccount.AccountEmail = updatedAccount.AccountEmail;

            if (!string.IsNullOrEmpty(updatedAccount.AccountPassword))
                existingAccount.AccountPassword = updatedAccount.AccountPassword;

            await _accountRepository.UpdateAccountAsync(existingAccount);
            await _accountRepository.SaveChangesAsync();  // Đảm bảo thay đổi được lưu

            return existingAccount;
        }


        public async Task<IEnumerable<SystemAccount>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        public Task<SystemAccount> CreateAccountAsync(SystemAccount account)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteAccountAsync(short accountId)
        {
            return await _accountRepository.DeleteAccountAsync(accountId);
        }


    }
}