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
    
    }
}
