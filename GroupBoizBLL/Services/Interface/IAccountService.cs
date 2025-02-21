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

        Task<ResponseDTO> GetAllAccountsAsync();
        //Task<bool> DeleteAccountAsync(short id);
        Task<ResponseDTO> DeleteAccountAsync(short accountId);
        Task<ResponseDTO> CreateAccountAsync(SystemAccountDTO accountDto);
        Task<ResponseDTO> UpdateAccountAsync(SystemAccountDTO updatedAccount);


    }
}
