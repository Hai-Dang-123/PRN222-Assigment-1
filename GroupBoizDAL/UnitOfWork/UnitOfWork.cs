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
        CategoryRepo = new CategoryRepository(_context);
        TagRepo = new TagRepository(_context);
        NewsArticleRepo = new NewsArticleRepository(_context);
            
        }
        
        public IAccountRepository AccountRepo { get; set; }
        public ITokenRepository TokenRepo { get; set; }
        public ICategoryRepository CategoryRepo { get; set; }
        public ITagRepository TagRepo { get; set; }
    public INewsArticleRepository NewsArticleRepo { get; set; }
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


