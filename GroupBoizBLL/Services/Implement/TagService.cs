using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;
using GroupBoizDAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

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
                

                var query = _unitOfWork.TagRepo.GetAll();
               

                var tags = await query.ToListAsync();
                
                if (tags == null || !tags.Any())
                {
                   
                    return new ResponseDTO("No tags found", 404, false);
                }

                var tagDTOList = tags.Select(c => new TagDTO
                {
                    TagId = c.TagId,
                    TagName = c.TagName,
                    Note = c.Note,
                }).ToList();

               

                return new ResponseDTO("Tags found successfully", 200, true, tagDTOList);
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
        public async Task<ResponseDTO> GetMaxTagIdAsync()
        {
            try
            {
                var maxTagId = await _unitOfWork.TagRepo.GetMaxTagIdAsync();
                return new ResponseDTO("Max Tag ID fetched successfully", 200, true, maxTagId);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> CreateAsync(TagDTO tag)
        {
            try
            {
                var maxTagId = await _unitOfWork.TagRepo.GetMaxTagIdAsync(); 
                var newTagId = maxTagId + 1;

                var newTag = new Tag
                {
                    TagId = newTagId, 
                    TagName = tag.TagName,
                    Note = tag.Note
                };

                var isCreated = await _unitOfWork.TagRepo.CreateAsync(newTag);
                if (!isCreated)
                    return new ResponseDTO("Failed to create tag", 400, false);

                return new ResponseDTO("Tag created successfully", 201, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }


        public async Task<ResponseDTO> UpdateTag(TagDTO tag)
        {
            try
            {
                var existingTag = await _unitOfWork.TagRepo.GetByIdAsync(tag.TagId);
                if (existingTag == null)
                    return new ResponseDTO("Tag not found", 404, false);

                existingTag.TagName = tag.TagName;
                existingTag.Note = tag.Note;

                await _unitOfWork.TagRepo.UpdateAsync(existingTag);
                await _unitOfWork.SaveChangeAsync();

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
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Tag deleted successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }
    }

}
