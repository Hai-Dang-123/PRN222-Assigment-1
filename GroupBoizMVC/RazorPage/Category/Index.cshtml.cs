using GroupBoizBLL.Hubs;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupBoizMVC.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IHubContext<AllHub> _hubContext;

        public List<CategoryDTO> Categories { get; set; } = new();

        public IndexModel(ICategoryService categoryService, IHubContext<AllHub> hubContext)
        {
            _categoryService = categoryService;
            _hubContext = hubContext;
        }

        // Load all categories when the page is accessed
        public async Task OnGet()
        {
            var response = await _categoryService.GetAll();
            if (response.IsSuccess)
            {
                Categories = (List<CategoryDTO>)response.Result;
            }
        }

        // Handle category creation
        public async Task<IActionResult> OnPostCreateAsync([FromBody] CategoryDTO categoryDto)
        {
            var response = await _categoryService.Create(categoryDto);
            return new JsonResult(response);
        }

        // Handle category updates
        public async Task<IActionResult> OnPostUpdateAsync([FromBody] CategoryDTO categoryDto)
        {
            var response = await _categoryService.UpdateCategory(categoryDto);
            return new JsonResult(response);
        }

        // Handle category deletion
        public async Task<IActionResult> OnPostDeleteAsync([FromBody] DeleteModel model)
        {
            var response = await _categoryService.Delete(model.CategoryId);
            return new JsonResult(response);
        }

        // Model for binding the delete request payload
        public class DeleteModel
        {
            public short CategoryId { get; set; }
        }
        public async Task<IActionResult> OnPostUpdateStatusAsync([FromBody] UpdateStatusModel model)
        {
            var response = await _categoryService.UpdateCategoryStatus(model.CategoryId, model.IsActive);
            return new JsonResult(response);
        }

        public class UpdateStatusModel
        {
            public short CategoryId { get; set; }
            public bool IsActive { get; set; }
        }
    }
}