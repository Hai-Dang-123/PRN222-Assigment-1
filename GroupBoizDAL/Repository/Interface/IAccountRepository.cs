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
        Task<SystemAccount> FindByEmailAsync(string email);
        Task<short> GetMaxAccountIdAsync();
        Task<SystemAccount> GetByShortIdAsync(short id);
    }
}
