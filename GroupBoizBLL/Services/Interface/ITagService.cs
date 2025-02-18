using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizCommon.DTO;

namespace GroupBoizBLL.Services.Interface
{
    public interface ITagService
    {
        Task<ResponseDTO> GetAllTags();
    }
}
