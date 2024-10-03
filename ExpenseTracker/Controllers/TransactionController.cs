using ExpenseTracker.Data;
using ExpenseTracker.ViewModel;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Collections;

namespace ExpenseTracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public TransactionController(AppDbContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index(string sortOrder, string searchString, DateTime? startDate=null, DateTime? endDate=null, int currentPage=1)
        {
            var userId = GetCurrentUserId();

            var transactionsQ = _context.Transaction
                                            .Where(t => t.AppUserId == userId)
                                            .Include(t => t.Category)
                                            .OrderByDescending(t => t.Date)
                                            .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                transactionsQ = transactionsQ.Where(t => t.Category!.Name.ToLower().Contains(searchString.ToLower()));
            }
            
            if(startDate!=null || endDate!=null)
            {
                transactionsQ = transactionsQ.Where(t => t.Date >= startDate && t.Date <= endDate);
            }

            int pageSize = 5;
            int totalPages = (int)Math.Ceiling((decimal)transactionsQ.Count() / pageSize);

            transactionsQ = transactionsQ
                            .Skip((currentPage - 1) * pageSize)
                            .Take(pageSize);

            switch (sortOrder)
            {
                case "date_asc":
                    transactionsQ = transactionsQ.OrderBy(t => t.Date);
                    break;
                case "category_desc":
                    transactionsQ = transactionsQ.OrderByDescending(t => t.Category!.Name);
                    break;
                case "category_asc":
                    transactionsQ = transactionsQ.OrderBy(t => t.Category!.Name);
                    break;

                case "amount_desc":
                    transactionsQ = transactionsQ.OrderByDescending(t => t.Amount);
                    break;

                case "amount_asc":
                    transactionsQ = transactionsQ.OrderBy(t => t.Amount);
                    break;

                default:
                    break;
            }

            var transactionVM = new TransactionViewModel()
            {
                DateSortParam = string.IsNullOrEmpty(sortOrder) ? "date_asc" : "",
                CategorySortParam = sortOrder == "category_asc" ? "category_desc" : "category_asc",
                AmountSortParam = sortOrder == "amount_asc" ? "amount_desc" : "amount_asc",
                SearchString = searchString,
                StartDate = startDate,
                EndDate = endDate,
                PageSize = pageSize,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                Transactions = transactionsQ.ToList(),
                ResultMsg = transactionsQ.Count() == 0 ? "No data available." : "",
                SortOrder = sortOrder
            };
            return View(transactionVM);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateTransactionViewModel createTransactionVM = new()
            {
                Categories = await GetCategorySelectList(),
            };

            return View(createTransactionVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionViewModel createTransactionVM)
        {
            if (!ModelState.IsValid)
            {
                createTransactionVM.Categories = await GetCategorySelectList();
                return View(createTransactionVM);
            }

            var transaction = new Transaction
            {
                Date = createTransactionVM.Date,
                Type = createTransactionVM.Type,
                Note = createTransactionVM.Note,
                Amount = createTransactionVM.Amount,
                CategoryId = createTransactionVM.CategoryId,
                AppUserId = GetCurrentUserId()
            };
            await _context.Transaction.AddAsync(transaction);
            await _context.SaveChangesAsync();
            TempData["success"] = "Transaction added successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var userId = GetCurrentUserId();
            var transaction = await _context.Transaction
                                            .Include(t => t.Category)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(t => t.Id == id && t.AppUserId == userId);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var userId = GetCurrentUserId();
            var transaction = await _context.Transaction
                                                .Include(t => t.Category)       
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(t => t.Id == id && t.AppUserId == userId);
            if (transaction == null)
            {
                return NotFound();
            }

            var editTransactionVM = new EditTransactionViewModel
            {
                Id = id,
                Date = transaction.Date,
                Type = transaction.Type,
                Note = transaction.Note,
                Amount = transaction.Amount,
                CategoryId = transaction.CategoryId,
                Categories = await GetCategorySelectList()
            };
            return View(editTransactionVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTransactionViewModel editTransactionVM)
        {
            if (!ModelState.IsValid)
            {
                editTransactionVM.Categories = await GetCategorySelectList();
                return View(editTransactionVM);
            }

            var transaction = await _context.Transaction.FindAsync(editTransactionVM.Id);

            var userId = GetCurrentUserId();
            if (userId != transaction!.AppUserId)
            {
                return NotFound();
            }

            if(transaction == null)
            {
                return NotFound();
            }


            transaction.Date = editTransactionVM.Date;
            transaction.Type = editTransactionVM.Type;
            transaction.Note = editTransactionVM.Note;
            transaction.Amount = editTransactionVM.Amount;
            transaction.CategoryId = editTransactionVM.CategoryId;

            await _context.SaveChangesAsync();
            TempData["success"] = "Transaction updated successfully";
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);

            var userId = GetCurrentUserId();
            if (userId != transaction!.AppUserId)
            {
                return Json(new { success = false, message = "Something went wrong" });
            }

            if (transaction == null)
            {
                return Json(new { success = false, message = "Transaction not found" });
            }
            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Transaction deleted successfully" });
        }


        public async Task<IEnumerable<SelectListItem>> GetCategorySelectList()
        {
            var userId = GetCurrentUserId();
            var categories = await _context.Category
                                        .Where(c => c.AppUserId == userId)      
                                        .AsNoTracking()
                                        .ToListAsync();

            var categorySelectList = categories.Select(cat => new SelectListItem
            {
                Value = cat.Id.ToString(),
                Text = cat.Name,
            });
            return categorySelectList;
        }


        public string GetCurrentUserId()
        {
            var userId = _userManager.GetUserId(User);
            return userId!;
        }
    }
}
