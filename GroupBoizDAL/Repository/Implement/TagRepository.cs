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
        public async Task<int> GetMaxTagIdAsync()
        {
            return await _context.Tag.MaxAsync(t => (int?)t.TagId) ?? 0;
        }

        public async Task<bool> CreateAsync(Tag tag)
        {
            await _context.Tag.AddAsync(tag);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds)
        {
            return await _context.Tag
                                 .Where(tag => tagIds.Contains(tag.TagId))
                                 .ToListAsync();
        }


    }
}
