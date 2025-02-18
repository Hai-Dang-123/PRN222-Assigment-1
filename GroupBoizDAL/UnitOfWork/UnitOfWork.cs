using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizDAL.Data;
using GroupBoizDAL.Repository.Implement;
using GroupBoizDAL.Repository.Interface;


namespace GroupBoizDAL.UnitOfWork;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly FUNewsManagementContext _context;

        public UnitOfWork (FUNewsManagementContext context)
        {
            _context = context;
        
        AccountRepo = new AccountRepository(_context);
        TokenRepo = new TokenRepository(_context);
        NewsRepo = new NewsRepository(_context);
    }
        
        public IAccountRepository AccountRepo { get; set; }
        public ITokenRepository TokenRepo { get; set; }
    public INewsRepository NewsRepo { get; set; }
    public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<bool> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }


