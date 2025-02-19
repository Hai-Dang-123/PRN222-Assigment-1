using GroupBoizBLL.Services.Implement;
using GroupBoizBLL.Services.Interface;
using GroupBoizDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupBoizMVC.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // Hiển thị danh sách Tag
        public async Task<IActionResult> Index()
        {
            var response = await _tagService.GetAllTags();

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            ViewBag.ErrorMessage = response.Message;
            return View(new List<Tag>());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Tag tag)
        {
            if (tag == null || string.IsNullOrWhiteSpace(tag.TagName))
            {
                return Json(new { success = false, message = "Tag name cannot be empty!" });
            }

            // 🔹 Gọi Service để lấy TagID lớn nhất rồi +1
            int maxId = await _tagService.GetMaxTagIdAsync();
            tag.TagId = maxId + 1; // Gán ID mới

            try
            {
                bool isCreated = await _tagService.CreateAsync(tag);

                if (isCreated)
                {
                    return Json(new { success = true, message = "Tag created successfully!" });
                }

                return Json(new { success = false, message = "Failed to create tag!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message, details = ex.InnerException?.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Tag tag)
        {
            if (tag == null || tag.TagId <= 0)  // Sửa TagId -> TagID
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            // Kiểm tra xem Tag có tồn tại không
            var existingTag = await _tagService.GetById(tag.TagId);
            if (existingTag == null)
            {
                return Json(new { success = false, message = "Tag not found!" });
            }

            var response = await _tagService.UpdateTag(tag);

            if (response.IsSuccess)
            {
                return Json(new { success = true, message = "Tag updated successfully!" });
            }

            return Json(new { success = false, message = response.Message });
        }

        // Xóa category
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int tagId) // 🔹 Nhận trực tiếp số nguyên
        {
            var response = await _tagService.Delete((int)tagId);

            return Json(new { success = response.IsSuccess, message = response.IsSuccess ? "Category deleted successfully!" : response.Message });
        }
    }
}
