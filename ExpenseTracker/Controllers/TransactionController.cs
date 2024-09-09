using ExpenseTracker.Data;
using ExpenseTracker.ViewModel;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Enum;
using Microsoft.Identity.Client;

namespace ExpenseTracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transaction
                                        .Include(t => t.Category)
                                        .AsNoTracking()
                                        .ToListAsync();
            return View(transactions);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateTransactionViewModel createTransactionVM = new()
            {
                Categories = await _context.Category
                                    .AsNoTracking()
                                    .ToListAsync()
            };
            return View(createTransactionVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionViewModel createTransactionVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createTransactionVM);
            }

            var transaction = new Transaction
            {
                Date = createTransactionVM.Date,
                Type = createTransactionVM.Type,
                Note = createTransactionVM.Note,
                Amount = createTransactionVM.Amount,
                CategoryId = createTransactionVM.CategoryId,
            };
            await _context.Transaction.AddAsync(transaction);
            await _context.SaveChangesAsync();
            TempData["success"] = "Transaction added successfully";
            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public async Task<IActionResult> View(int id)
        //{
        //    var transaction = await GetTransactionById(id);
        //    return View(transaction);
        //}

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(x => x.Id == id);
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
            };
            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTransactionViewModel editTransactionVM)
        {
            var transaction = await _context.Transaction.FindAsync(editTransactionVM.Id);

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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);

            if(transaction == null)
            {
                return NotFound();
            }

            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            TempData["success"] = "Transaction removed successfully";
            return RedirectToAction("Index");
        }
    }
}
