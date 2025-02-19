using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;

namespace GroupBoizBLL.Services.Interface
{
    public interface ITagService
    {
        Task<ResponseDTO> GetAllTags();
        Task<ResponseDTO> GetById(int tagId);
        Task<int> GetMaxTagIdAsync();
        Task<bool> CreateAsync(Tag tag);
        Task<ResponseDTO> UpdateTag(Tag tag);
        Task<ResponseDTO> Delete(int tagId);
    }
}
