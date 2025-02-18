using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
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

    public async Task<ResponseDTO> GetAll()
        {
            try
            {
                var newsList = await _unitOfWork.CategoryRepo.ToListAsync();  // Lấy danh sách tin tức từ database

                if (newsList == null || !newsList.Any())
                {
                    return new ResponseDTO("No news found", 404, false); // Nếu không có tin tức, trả về thông báo và mã 404
                }
                Console.WriteLine(newsList);

                return new ResponseDTO("News found successfully", 200, true, newsList); // Trả về thành công, mã 200 và dữ liệu tin tức
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false); // Nếu có lỗi, trả về thông báo lỗi và mã 500
            }
        }
    }
    
}
