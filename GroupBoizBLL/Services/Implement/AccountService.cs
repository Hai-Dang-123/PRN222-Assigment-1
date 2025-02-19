using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;
using GroupBoizDAL.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace GroupBoizBLL.Services.Implement
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Lấy thông tin tài khoản theo ID
        public async Task<ResponseDTO> GetById(short id)
        {
            try
            {
                var account = await _unitOfWork.AccountRepo.GetByShortIdAsync(id);
                if (account == null)
                    return new ResponseDTO("Account not found", 404, false);

                return new ResponseDTO("Account retrieved successfully", 200, true, account);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        // Cập nhật thông tin tài khoản
        public async Task<ResponseDTO> UpdateAccount(SystemAccount account)
        {
            try
            {
                var existingAccount = await _unitOfWork.AccountRepo.GetByShortIdAsync(account.AccountId);
                if (existingAccount == null)
                    return new ResponseDTO("Account not found", 404, false);

                existingAccount.AccountName = account.AccountName;
                existingAccount.AccountEmail = account.AccountEmail;
                existingAccount.AccountRole = account.AccountRole;
                existingAccount.AccountPassword = account.AccountPassword;

                _unitOfWork.AccountRepo.UpdateAsync(existingAccount);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO("Account updated successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }
        public async Task<IEnumerable<SystemAccount>> GetAllAccountsAsync()
        {
            return await _unitOfWork.AccountRepo.GetAllAsync();
        }

        public Task<SystemAccount> CreateAccountAsync(SystemAccount account)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteAccountAsync(short accountId)
        {
            return await _unitOfWork.AccountRepo.DeleteAccountAsync(accountId);
        }
        public async Task<SystemAccount> UpdateAccountAsync(short accountId, SystemAccount updatedAccount)
        {
            var existingAccount = await _unitOfWork.AccountRepo.FindByIdAsync(accountId);
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

            await _unitOfWork.AccountRepo.UpdateAccountAsync(existingAccount);
            await _unitOfWork.AccountRepo.SaveChangesAsync();  // Đảm bảo thay đổi được lưu

            return existingAccount;
        }
    }
}
