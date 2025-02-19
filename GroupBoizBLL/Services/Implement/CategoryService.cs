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

                return new ResponseDTO("Categories retrieved successfully", 200, true, categoryList);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        // Thêm Category
        public async Task<ResponseDTO> Create(Category category)
        {
            try
            {
                if (category == null)
                {
                    return new ResponseDTO("Invalid category data", 400, false);
                }

                await _unitOfWork.CategoryRepo.AddAsync(category);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO("Category created successfully", 201, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> UpdateCategory(Category category)
        {
            try
            {
                var existingCategory = await _unitOfWork.CategoryRepo.GetByIdAsync(category.CategoryId);
                if (existingCategory == null)
                    return new ResponseDTO("Category not found", 404, false);

                // Cập nhật thông tin từ category nhận vào
                existingCategory.CategoryName = category.CategoryName;
                existingCategory.CategoryDesciption = category.CategoryDesciption;
                existingCategory.IsActive = category.IsActive;

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
