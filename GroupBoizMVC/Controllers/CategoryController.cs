using GroupBoizBLL.Services.Implement;
using GroupBoizDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroupBoizMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        // Inject CategoryService vào controller thông qua constructor
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Action để hiển thị danh sách Category
        public async Task<IActionResult> Index()
        {
            // Gọi phương thức GetAll từ CategoryService để lấy dữ liệu
            var response = await _categoryService.GetAll();

            if (response.IsSuccess)
            {
                // Nếu thành công, truyền dữ liệu vào View
                return View(response.Result); // response.Data sẽ là danh sách Category
            }
            else
            {
                // Nếu có lỗi, truyền thông báo lỗi vào ViewBag
                ViewBag.ErrorMessage = response.Message;
                return View(new List<Category>()); // Trả về danh sách Category rỗng nếu có lỗi
            }
        }
    }
}
