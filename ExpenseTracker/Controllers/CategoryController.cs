using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public CategoryController(AppDbContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int? result)
        {
            var userId = GetCurrentUserId();
            var categories = await _context.Category
                                    .Where(c => c.AppUserId == userId)
                                    .OrderBy(c => c.Name)
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
                    Name = createCategoryVM.Name,
                    AppUserId = GetCurrentUserId()
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

            var userId = GetCurrentUserId();
            var category = await _context.Category
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(c => c.Id == id && c.AppUserId == userId);
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

            var userId = GetCurrentUserId();
            if (userId != Category!.AppUserId)
            {
                return NotFound();
            }

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
            var category = await _context.Category.FindAsync(id);

            var userId = GetCurrentUserId();
            if (userId != category!.AppUserId)
            {
                return Json(new { success = false, message = "Something went wrong" });
            }

            if (category == null)
            {
                return Json(new { success = false, message = "Category not found" });
            }
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Category deleted successfully" });
        }


        public string GetCurrentUserId()
        {
            var userId = _userManager.GetUserId(User);
            return userId!;
        }
    }
}
