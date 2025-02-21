using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;

namespace GroupBoizBLL.Services.Interface
{
    public interface ICategoryService
    {
        Task<ResponseDTO> GetAll();
        Task<ResponseDTO> Create(CategoryDTO categoryDto);
        Task<ResponseDTO> UpdateCategory(CategoryDTO categoryDto);
        Task<ResponseDTO> Delete(short categoryId);
    }
}
