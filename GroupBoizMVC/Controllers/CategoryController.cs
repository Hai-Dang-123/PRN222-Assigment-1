using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupBoizMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Hiển thị danh sách category
        public async Task<IActionResult> Index()
        {
            var response = await _categoryService.GetAll();

            if (response.IsSuccess)
            {
                return View(response.Result);  // Trả về danh sách CategoryDTO
            }

            ViewBag.ErrorMessage = response.Message;
            return View(new List<CategoryDTO>());
        }

        // Thêm mới category
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null)
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            var response = await _categoryService.Create(categoryDto);

            return Json(new { success = response.IsSuccess, message = response.Message });
        }

        // Cập nhật category
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null || categoryDto.CategoryId == 0)
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            var response = await _categoryService.UpdateCategory(categoryDto);

            return Json(new { success = response.IsSuccess, message = response.Message });
        }

        // Xóa category
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int categoryId)
        {
            var response = await _categoryService.Delete((short)categoryId);

            return Json(new { success = response.IsSuccess, message = response.Message });
        }
    }
}
