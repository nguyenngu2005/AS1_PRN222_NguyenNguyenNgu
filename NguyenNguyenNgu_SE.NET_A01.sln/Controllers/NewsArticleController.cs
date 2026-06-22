using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Repositories.Models;
using Services;
using System.Collections.Generic;
using System.Linq;

namespace NguyenNguyenNguMVC.Controllers
{
    public class NewsArticleController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public NewsArticleController(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        private bool IsStaff()
        {
            return HttpContext.Session.GetInt32("Role") == 1;
        }

        // GET: Hiển thị danh sách
        public IActionResult Index(string keyword)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Index", "Login");
            }

            // Lấy danh sách bài viết
            ViewBag.Keyword = keyword;
            var articles = _newsArticleService.SearchNewsArticles(keyword);

            // Lấy danh sách Category và truyền sang View qua ViewBag để làm thẻ <select>
            ViewBag.Categories = _categoryService.GetCategories();

            // --- BẮT ĐẦU PHẦN QUẢN LÝ TAG THEO ĐỀ BÀI ---
            // 1. Lấy danh sách các tag chưa thuộc bài viết nào (cho Form Tạo mới)
            ViewBag.AvailableTags = _tagService.GetAvailableTags();

            // 2. Lấy danh sách ID của các tag đã bị chiếm dụng bởi bài viết khác
            ViewBag.TakenTagIds = _tagService.GetTakenTagIds();

            // 3. Lấy toàn bộ danh sách Tag (cho Form Cập nhật)
            ViewBag.AllTags = _tagService.GetTags();
            // --- KẾT THÚC PHẦN QUẢN LÝ TAG ---

            return View(articles);
        }

        // GET: Lịch sử bài viết của Staff đang đăng nhập
        public IActionResult History(string keyword)
        {
            int? accountId = HttpContext.Session.GetInt32("AccountId");

            if (!IsStaff() || accountId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Keyword = keyword;
            var articles = _newsArticleService.SearchNewsArticlesByCreator((short)accountId.Value, keyword);
            return View(articles);
        }

        // POST: Tạo mới bài viết kèm theo Tags
        [HttpPost]
        public IActionResult Create(NewsArticle article, List<int> selectedTagIds)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Index", "Login");
            }

            // Lấy ID người đang đăng nhập (Staff/Admin) từ Session gán vào CreatedById
            int? accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId.HasValue)
            {
                article.CreatedById = (short)accountId.Value;
            }

            // Gán các Tag được chọn (đã lọc các tag chưa được chiếm dụng ở view)
            if (selectedTagIds != null && selectedTagIds.Any())
            {
                var selectedDistinctTagIds = selectedTagIds.Distinct().ToList();
                var availableTagIds = _tagService.GetAvailableTags().Select(t => t.TagId).ToHashSet();

                if (selectedDistinctTagIds.Any(tagId => !availableTagIds.Contains(tagId)))
                {
                    TempData["Error"] = "Một hoặc nhiều tag đã thuộc bài viết khác, không thể gán lại.";
                    return RedirectToAction(nameof(Index));
                }

                article.Tags = _tagService.GetTagsByIds(selectedDistinctTagIds);
                if (article.Tags.Count != selectedDistinctTagIds.Count)
                {
                    TempData["Error"] = "Một hoặc nhiều tag không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu bài viết không hợp lệ. Vui lòng kiểm tra lại.";
                return RedirectToAction(nameof(Index));
            }

            _newsArticleService.AddNewsArticle(article);
            TempData["Success"] = "Tạo bài viết thành công!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Cập nhật bài viết kèm theo Tags
        [HttpPost]
        public IActionResult Edit(NewsArticle article, List<int> selectedTagIds)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Index", "Login");
            }

            var existingArticle = _newsArticleService.GetNewsArticleById(article.NewsArticleId);
            if (existingArticle == null)
            {
                TempData["Error"] = "Không tìm thấy bài viết cần cập nhật.";
                return RedirectToAction(nameof(Index));
            }

            // Nạp danh sách các Tag được chọn để truyền sang Service
            if (selectedTagIds != null)
            {
                var selectedDistinctTagIds = selectedTagIds.Distinct().ToList();
                var currentTagIds = existingArticle.Tags.Select(t => t.TagId).ToHashSet();
                var takenTagIds = _tagService.GetTakenTagIds().ToHashSet();

                if (selectedDistinctTagIds.Any(tagId => takenTagIds.Contains(tagId) && !currentTagIds.Contains(tagId)))
                {
                    TempData["Error"] = "Một hoặc nhiều tag đã thuộc bài viết khác, không thể gán lại.";
                    return RedirectToAction(nameof(Index));
                }

                article.Tags = _tagService.GetTagsByIds(selectedDistinctTagIds);
                if (article.Tags.Count != selectedDistinctTagIds.Count)
                {
                    TempData["Error"] = "Một hoặc nhiều tag không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu bài viết không hợp lệ. Vui lòng kiểm tra lại.";
                return RedirectToAction(nameof(Index));
            }

            _newsArticleService.UpdateNewsArticle(article);
            TempData["Success"] = "Cập nhật bài viết thành công!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Xóa bài viết
        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Index", "Login");
            }

            _newsArticleService.DeleteNewsArticle(id);
            TempData["Success"] = "Đã xóa bài viết thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
