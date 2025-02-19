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
    }
}
