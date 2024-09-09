using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Category
                                    .AsNoTracking()
                                    .ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel createCategoryVM)
        {
            if(ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = createCategoryVM.Name
                };
                await _context.Category.AddAsync(category);
                await _context.SaveChangesAsync();
                TempData["success"] = "Category added successfully";
                return RedirectToAction("Index");
            }
            return View(createCategoryVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if(id <= 0)
            {
                return NotFound();
            }
            var category = await _context.Category
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(c => c.Id == id);
            if(category == null)
            {
                return NotFound();
            }

            var editCategoryVM = new EditCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
            };
            return View(editCategoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryViewModel editCategoryVM)
        {
            if (!ModelState.IsValid)
            {
                return View(editCategoryVM);
            }

            var Category = await _context.Category.FindAsync(editCategoryVM.Id);

            if (Category == null)
            {
                return NotFound();
            }

            Category.Name = editCategoryVM.Name;

            await _context.SaveChangesAsync();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var Category = await _context.Category.FindAsync(1000);
            if (Category == null)
            {
                return Json(new { success = false, message = "Category not found" });
            }
            _context.Category.Remove(Category);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Category deleted successfully" });
        }
    }
}
