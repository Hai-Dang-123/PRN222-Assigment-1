using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;
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
        public async Task<ResponseDTO> GetById(int tagId)
        {
            try
            {
                var tag = await _unitOfWork.TagRepo.GetByIdAsync(tagId);
                if (tag == null)
                    return new ResponseDTO("Tag not found", 404, false);

                return new ResponseDTO("Tag fetched successfully", 200, true, tag);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }
        public async Task<ResponseDTO> Create(Tag tag)
        {
            try
            {
                await _unitOfWork.TagRepo.AddAsync(tag);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO("Tag created successfully", 201, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }
        public async Task<ResponseDTO> UpdateTag(Tag tag)
        {
            try
            {
                var existingTag = await _unitOfWork.TagRepo.GetByIdAsync(tag.TagId);
                if (existingTag == null)
                    return new ResponseDTO("Tag not found", 404, false);

                existingTag.TagName = tag.TagName;
                existingTag.Note = tag.Note;

                _unitOfWork.TagRepo.UpdateAsync(existingTag);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO("Tag updated successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> Delete(int tagId)
        {
            try
            {
                var tag = await _unitOfWork.TagRepo.GetByIdAsync(tagId);
                if (tag == null)
                    return new ResponseDTO("Tag not found", 404, false);

                _unitOfWork.TagRepo.Delete(tag);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO("Tag deleted successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }
    }

}
