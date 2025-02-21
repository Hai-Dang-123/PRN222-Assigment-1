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

        public async Task<ResponseDTO> GetAllAccountsAsync()
        {
            try
            {
                var accounts = await _unitOfWork.AccountRepo.GetAllAsync();  // Lấy danh sách SystemAccount từ DB

                if (accounts == null || !accounts.Any())
                {
                    return new ResponseDTO("No accounts found", 404, false);
                }

                // **Mapping từ Entity -> DTO**
                var accountDtoList = accounts.Select(account => new SystemAccountDTO
                {
                    AccountId = account.AccountId,
                    AccountName = account.AccountName,
                    AccountEmail = account.AccountEmail,
                    AccountRole = account.AccountRole,
                   
                }).ToList();

                Console.WriteLine($"✅ Mapped {accountDtoList.Count} accounts to DTO");

                return new ResponseDTO("Accounts retrieved successfully", 200, true, accountDtoList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }


        public async Task<ResponseDTO> CreateAccountAsync(SystemAccountDTO accountDto)
        {
            try
            {
                var newAccount = new SystemAccount
                {
                    AccountName = accountDto.AccountName,
                    AccountEmail = accountDto.AccountEmail,
                    AccountRole = accountDto.AccountRole,
                    AccountPassword = accountDto.AccountPassword
                };

                await _unitOfWork.AccountRepo.AddAsync(newAccount);
                await _unitOfWork.AccountRepo.SaveChangesAsync();

                return new ResponseDTO("Account created successfully", 201, true, newAccount);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> DeleteAccountAsync(short accountId)
        {
            try
            {
                var isDeleted = await _unitOfWork.AccountRepo.DeleteAccountAsync(accountId);
                if (!isDeleted)
                    return new ResponseDTO("Account not found", 404, false);

                return new ResponseDTO("Account deleted successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> UpdateAccountAsync(SystemAccountDTO updatedAccount)
        {
            try
            {
                Console.WriteLine(updatedAccount.AccountId);
                var existingAccount = await _unitOfWork.AccountRepo.FindByIdAsync(updatedAccount.AccountId);
                if (existingAccount == null)
                {
                    return new ResponseDTO("Account not found", 404, false);
                }

                if (!string.IsNullOrEmpty(updatedAccount.AccountName))
                    existingAccount.AccountName = updatedAccount.AccountName;

                if (!string.IsNullOrEmpty(updatedAccount.AccountEmail))
                    existingAccount.AccountEmail = updatedAccount.AccountEmail;

                if (!string.IsNullOrEmpty(updatedAccount.AccountPassword))
                    existingAccount.AccountPassword = updatedAccount.AccountPassword;

                existingAccount.AccountRole = updatedAccount.AccountRole;

                await _unitOfWork.AccountRepo.UpdateAccountAsync(existingAccount);
                await _unitOfWork.AccountRepo.SaveChangesAsync();

                return new ResponseDTO("Account updated successfully", 200, true, existingAccount);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

    }
}
