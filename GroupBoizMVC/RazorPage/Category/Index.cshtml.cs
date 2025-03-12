using GroupBoizBLL.Hubs;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.DTO;
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

        public async Task OnGet()
        {
            var response = await _categoryService.GetAll();
            if (response.IsSuccess)
            {
                Categories =(List<CategoryDTO>) response.Result;
            }
        }
    }
}
