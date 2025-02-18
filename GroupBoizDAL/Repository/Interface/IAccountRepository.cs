using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizDAL.Entities;

namespace GroupBoizDAL.Repository.Interface
{
    public interface IAccountRepository : IGenericRepository<SystemAccount>
    {
        Task<SystemAccount> CreateAccountAsync(SystemAccount account);
        Task<SystemAccount> FindByEmailAsync(string email);
        Task<SystemAccount> FindByIdAsync(short accountId);
        Task<short> GetMaxAccountIdAsync();
        Task UpdateAccountAsync(SystemAccount existingAccount);

        Task<IEnumerable<SystemAccount>> GetAllAsync();

    }
}
