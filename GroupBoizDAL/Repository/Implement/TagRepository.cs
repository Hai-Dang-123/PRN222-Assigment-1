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
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        private readonly FUNewsManagementContext _context;
        public TagRepository(FUNewsManagementContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Tag?> GetByIdAsync(int tagId)
        {
            return await _context.Tag.FindAsync(tagId);
        }

    }
}
