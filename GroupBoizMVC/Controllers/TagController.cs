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

        // Thêm mới Tag
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Tag tag)
        {
            if (tag == null)
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            var response = await _tagService.Create(tag);

            if (response.IsSuccess)
            {
                return Json(new { success = true, message = "Tag created successfully!" });
            }

            return Json(new { success = false, message = response.Message });
        }

        // Cập nhật Tag
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Tag tag)
        {
            if (tag == null || tag.TagId == 0)
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            var response = await _tagService.UpdateTag(tag);

            if (response.IsSuccess)
            {
                return Json(new { success = true, message = "Tag updated successfully!" });
            }

            return Json(new { success = false, message = response.Message });
        }

        // Xóa Tag
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] dynamic data)
        {
            if (data == null || data.tagId == null)
            {
                return Json(new { success = false, message = "Invalid request!" });
            }

            int tagId = (int)data.tagId;
            var response = await _tagService.Delete(tagId);

            if (response.IsSuccess)
            {
                return Json(new { success = true, message = "Tag deleted successfully!" });
            }

            return Json(new { success = false, message = response.Message });
        }
    }
}
