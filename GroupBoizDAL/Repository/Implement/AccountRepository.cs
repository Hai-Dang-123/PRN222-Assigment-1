using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizDAL.Data;
using GroupBoizDAL.Entities;
using GroupBoizDAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GroupBoizDAL.Repository.Implement
{
    public class AccountRepository : GenericRepository<SystemAccount>, IAccountRepository
    {
        private readonly FUNewsManagementContext _context;
        public AccountRepository(FUNewsManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SystemAccount> FindByEmailAsync(string email)
        {
            return await _context.SystemAccount.FirstOrDefaultAsync(u => u.AccountEmail == email);
        }
        public async Task<short> GetMaxAccountIdAsync()
        {
            // Lấy AccountID lớn nhất trong bảng SystemAccount
            var maxAccountId = await _context.SystemAccount
                                              .OrderByDescending(sa => sa.AccountId)
                                              .Select(sa => sa.AccountId)
                                              .FirstOrDefaultAsync();

            return maxAccountId;
        }
        public async Task<SystemAccount> CreateAccountAsync(SystemAccount account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            await _context.SystemAccount.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public Task<SystemAccount> FindByIdAsync(short accountId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAccountAsync(SystemAccount existingAccount)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAsync()
        {
            return await _context.SystemAccount.ToListAsync();
        }
    }
}
