using ExpenseTracker.Data;
using ExpenseTracker.Enum;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var income = await _context.Transaction
                                    .Where(t => t.AppUserId == userId && t.Type == TransactionType.Income)
                                    .ToListAsync();

            var expense = await _context.Transaction
                                    .Where(t => t.AppUserId == userId && t.Type == TransactionType.Expense)
                                    .ToListAsync();

            var totalIncome = income.Select(i => i.Amount).Sum();
            var totalExpense = expense.Select(i => i.Amount).Sum();
            var balance = totalIncome - totalExpense;

            var homeVM = new HomeViewModel()
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = balance
            };

            return View(homeVM);
        }

        public string GetCurrentUserId()
        {
            var userId = _userManager.GetUserId(User);
            return userId!;
        }
    }
}
