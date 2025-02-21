using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;
using GroupBoizDAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace GroupBoizBLL.Services.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Lấy danh sách Category
        public async Task<ResponseDTO> GetAll()
        {
            try
            {
                var categoryList = await _unitOfWork.CategoryRepo.GetAll().ToListAsync();

                if (!categoryList.Any())
                {
                    return new ResponseDTO("No categories found", 404, false);
                }

                // Chuyển đổi danh sách Category sang CategoryDTO
                var categoryDTOList = categoryList.Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDesciption = c.CategoryDesciption,
                    IsActive = c.IsActive
                }).ToList();

                return new ResponseDTO("Categories retrieved successfully", 200, true, categoryDTOList);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        // Thêm Category
        public async Task<ResponseDTO> Create(CategoryDTO categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    return new ResponseDTO("Invalid category data", 400, false);
                }

                // Chuyển đổi từ CategoryDTO sang Category
                var category = new Category
                {
                    CategoryName = categoryDto.CategoryName,
                    CategoryDesciption = categoryDto.CategoryDesciption,
                    IsActive = categoryDto.IsActive
                };

                await _unitOfWork.CategoryRepo.AddAsync(category);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO("Category created successfully", 201, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        // Cập nhật Category
        public async Task<ResponseDTO> UpdateCategory(CategoryDTO categoryDto)
        {
            try
            {
                var existingCategory = await _unitOfWork.CategoryRepo.GetByIdAsync(categoryDto.CategoryId);
                if (existingCategory == null)
                    return new ResponseDTO("Category not found", 404, false);

                // Cập nhật thông tin từ CategoryDTO nhận vào
                existingCategory.CategoryName = categoryDto.CategoryName;
                existingCategory.CategoryDesciption = categoryDto.CategoryDesciption;
                existingCategory.IsActive = categoryDto.IsActive;

                _unitOfWork.CategoryRepo.Update(existingCategory);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO("Category updated successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        // Xóa Category
        public async Task<ResponseDTO> Delete(short categoryId)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepo.GetByIdAsync(categoryId);
                if (category == null)
                {
                    return new ResponseDTO("Category not found", 404, false);
                }

                _unitOfWork.CategoryRepo.Delete(category);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO("Category deleted successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }
    }
}
