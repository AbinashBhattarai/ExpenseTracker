using ExpenseTracker.Data;
using ExpenseTracker.Enum;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Authorization;
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
            var user = await _userManager.GetUserAsync(User);

            var transactionsQ = _context.Transaction
                                            .Where(t => t.AppUserId == (user!.Id ?? ""))
                                            .Include(t => t.Category)
                                            .AsQueryable();

            var homeVM = new HomeViewModel()
            {
                IncomeAmounts = transactionsQ
                                    .Where(tq => tq.Type == TransactionType.Income)
                                    .Select(i => i.Amount).ToList(),
                ExpenseAmounts = transactionsQ
                                    .Where(tq => tq.Type == TransactionType.Expense)
                                    .Select(i => i.Amount).ToList(),
                Categories = transactionsQ
                                    .GroupBy(tq => tq.CategoryId)
                                    .Select(tq => tq.First().Category!.Name)
                                    .ToList(),
                CategoryAmounts = transactionsQ
                                    .GroupBy(tq => tq.CategoryId)
                                    .Select(tq => tq.Sum(x => x.Amount))
                                    .ToList(),
                TransactionDates = transactionsQ
                                    .OrderByDescending(tq => tq.Date)
                                    .Take(5)
                                    .OrderBy(tq => tq.Date)
                                    .Select(tq => tq.Date.ToShortDateString())
                                    .ToList(),
                Transactions = transactionsQ
                                    .OrderByDescending(tq => tq.Date)
                                    .Take(5)
                                    .ToList(),
               FullName = user!.FullName ?? ""
            };

            return View(homeVM);
        }
    }
}
