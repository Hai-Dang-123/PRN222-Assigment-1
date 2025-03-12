using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GroupBoizMVC.RazorPage.Tag
{
    public class IndexModel : PageModel
    {
        private readonly ITagService _tagService;

        public IndexModel(ITagService tagService)
        {
            _tagService = tagService;
        }
        public List<TagDTO> Tags { get; set; } = new List<TagDTO>();

        public async Task OnGet()
        {
            var response = await _tagService.GetAllTags();
            if (response.IsSuccess)
            {
                Tags = response.Result as List<TagDTO> ?? new List<TagDTO>(); 
            }
        }
        public async Task<JsonResult> OnPostCreate([FromBody] TagDTO tag)
        {
            if (tag == null || string.IsNullOrWhiteSpace(tag.TagName))
                return new JsonResult(new { success = false, message = "Tag name cannot be empty!" });

            var response = await _tagService.CreateAsync(tag);
            return new JsonResult(new { success = response.IsSuccess, message = response.IsSuccess ? "Tag created successfully!" : response.Message });
        }

        public async Task<JsonResult> OnPostUpdate([FromBody] TagDTO tag)
        {
            if (tag == null || tag.TagId <= 0)
                return new JsonResult(new { success = false, message = "Invalid tag data!" });

            var response = await _tagService.UpdateTag(tag);
            return new JsonResult(new { success = response.IsSuccess, message = response.IsSuccess ? "Tag updated successfully!" : response.Message });
        }

        public async Task<JsonResult> OnPostDelete([FromBody] int tagId)
        {
            var response = await _tagService.Delete(tagId);
            return new JsonResult(new { success = response.IsSuccess, message = response.IsSuccess ? "Tag deleted successfully!" : response.Message });
        }
    }
}
