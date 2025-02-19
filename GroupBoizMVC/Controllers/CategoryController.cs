using GroupBoizBLL.Services.Implement;
using GroupBoizBLL.Services.Interface;
using GroupBoizDAL.Entities;
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
                return View(response.Result);
            }

            ViewBag.ErrorMessage = response.Message;
            return View(new List<Category>());
        }

        // Thêm mới category
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (category == null)
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            var response = await _categoryService.Create(category);

            if (response.IsSuccess)
            {
                return Json(new { success = true, message = "Category created successfully!" });
            }

            return Json(new { success = false, message = response.Message });
        }

        // Cập nhật category
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Category category)
        {
            if (category == null || category.CategoryId == 0)
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            var response = await _categoryService.UpdateCategory(category);

            if (response.IsSuccess)
            {
                return Json(new { success = true, message = "Category updated successfully!" });
            }

            return Json(new { success = false, message = response.Message });
        }

        // Xóa category
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] dynamic data)
        {
            if (data == null || data.categoryId == null)
            {
                return Json(new { success = false, message = "Invalid request!" });
            }

            short categoryId = (short)data.categoryId;
            var response = await _categoryService.Delete(categoryId);

            if (response.IsSuccess)
            {
                return Json(new { success = true, message = "Category deleted successfully!" });
            }

            return Json(new { success = false, message = response.Message });
        }
    }
}
