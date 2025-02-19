using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizDAL.Data;
using GroupBoizDAL.Entities;
using GroupBoizDAL.Repository.Interface;

namespace GroupBoizDAL.Repository.Implement
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly FUNewsManagementContext _context;
        public CategoryRepository(FUNewsManagementContext context) : base(context)
        {
            _context = context;
        }
        // 🔥 Hàm mới để lấy Category bằng short ID
        public async Task<Category?> GetByIdAsync(short categoryId)
        {
            return await _context.Category.FindAsync(categoryId);
        }
        public void Update(Category category)
        {
            _context.Category.Update(category);
        }

    }
}
