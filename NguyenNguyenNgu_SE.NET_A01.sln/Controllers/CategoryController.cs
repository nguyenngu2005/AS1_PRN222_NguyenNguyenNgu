using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Repositories.Models;
using Services;

namespace NguyenNguyenNguMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        private bool IsStaff()
        {
            return HttpContext.Session.GetInt32("Role") == 1;
        }

        // GET: Hiển thị danh sách và Tìm kiếm
        public IActionResult Index(string keyword)
        {
            // Kiểm tra phân quyền: chỉ Staff được quản lý Category theo đề bài.
            if (!IsStaff())
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Keyword = keyword;
            var categories = _categoryService.SearchCategories(keyword);
            return View(categories);
        }

        // POST: Xử lý Thêm mới
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                _categoryService.AddCategory(category);
                TempData["Success"] = "Tạo danh mục thành công!";
            }
            else
            {
                TempData["Error"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Xử lý Cập nhật
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                _categoryService.UpdateCategory(category);
                TempData["Success"] = "Cập nhật danh mục thành công!";
            }
            else
            {
                TempData["Error"] = "Cập nhật thất bại.";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Xử lý Xóa
        [HttpPost]
        public IActionResult Delete(short id)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Index", "Login");
            }

            bool isDeleted = _categoryService.DeleteCategory(id);
            if (isDeleted)
            {
                TempData["Success"] = "Đã xóa danh mục thành công!";
            }
            else
            {
                // Thông báo lỗi nếu dính ràng buộc bài viết
                TempData["Error"] = "Không thể xóa! Danh mục này đang chứa bài viết.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
