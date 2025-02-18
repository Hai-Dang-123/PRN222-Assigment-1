using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using GroupBoizDAL.UnitOfWork;

namespace GroupBoizBLL.Services.Implement
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetAllTags()
        {
            try
            {
                var tags = await _unitOfWork.TagRepo.ToListAsync();  // Lấy danh sách tags từ database

                if (tags == null || !tags.Any())
                {
                    return new ResponseDTO("No tags found", 404, false);
                }

                return new ResponseDTO("Tags found successfully", 200, true, tags);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }
    }

}
