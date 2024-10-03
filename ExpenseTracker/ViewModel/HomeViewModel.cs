
using ExpenseTracker.Models;

namespace ExpenseTracker.ViewModel
{
    public class HomeViewModel
    {
        public string FullName { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; } = [];
        public ICollection<int> IncomeAmounts { get; set; } = [];
        public ICollection<int> ExpenseAmounts { get; set; } = [];
        public ICollection<string> Categories { get; set; } = [];
        public ICollection<int> CategoryAmounts { get; set; } = [];
        public ICollection<string> TransactionDates { get; set; } = [];

        public int TotalIncome { get { return IncomeAmounts.Sum(); } }
        public int TotalExpense { get { return ExpenseAmounts.Sum(); } }
        public int TotalBalance { get { return (TotalIncome - TotalExpense); } }
    }
}
