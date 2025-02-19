using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;

namespace GroupBoizBLL.Services.Interface
{
    public interface IAccountService
    {
        Task<ResponseDTO> GetById(short id);
        Task<ResponseDTO> UpdateAccount(SystemAccount account);
        Task<IEnumerable<SystemAccount>> GetAllAccountsAsync();
        //Task<bool> DeleteAccountAsync(short id);
        Task<bool> DeleteAccountAsync(short accountId);
        Task<SystemAccount> CreateAccountAsync(SystemAccount account);
        Task<SystemAccount> UpdateAccountAsync(short accountId, SystemAccount updatedAccount);
       
    }
}
