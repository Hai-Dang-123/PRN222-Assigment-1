using GroupBoizDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBoizBLL.Services.Interface
{
    public interface IAccountService
    {
        Task<SystemAccount> CreateAccountAsync(SystemAccount account);
        Task<SystemAccount> UpdateAccountAsync(short accountId, SystemAccount updatedAccount);
        Task<IEnumerable<SystemAccount>> GetAllAccountsAsync();
    }
}
