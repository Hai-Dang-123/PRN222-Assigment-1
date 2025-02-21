using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizDAL.Entities;

namespace GroupBoizDAL.Repository.Interface
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<Tag?> GetByIdAsync(int tagId);
        Task<int> GetMaxTagIdAsync();
        Task<bool> CreateAsync(Tag tag);
        Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds);
    }
}
