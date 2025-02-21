using GroupBoizBLL.Services.Implement;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
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
        public async Task<IActionResult> Create([FromBody] TagDTO tag) // 🔹 Fix: Dùng TagDTO thay vì Tag
        {
            if (tag == null || string.IsNullOrWhiteSpace(tag.TagName))
            {
                return Json(new { success = false, message = "Tag name cannot be empty!" });
            }

            try
            {
                var response = await _tagService.CreateAsync(tag); // 🔹 Fix: Trả về ResponseDTO

                return Json(new { success = response.IsSuccess, message = response.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message, details = ex.InnerException?.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] TagDTO tag)
        {
            if (tag == null || tag.TagId <= 0)
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            try
            {
                var existingTag = await _tagService.GetById(tag.TagId);
                if (!existingTag.IsSuccess)  
                {
                    return Json(new { success = false, message = "Tag not found!" });
                }

                var response = await _tagService.UpdateTag(tag);

                return Json(new { success = response.IsSuccess, message = response.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // Xóa Tag
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int tagId)
        {
            try
            {
                var response = await _tagService.Delete(tagId);
                return Json(new { success = response.IsSuccess, message = response.IsSuccess ? "Tag deleted successfully!" : response.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}
