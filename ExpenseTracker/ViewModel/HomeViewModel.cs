
using ExpenseTracker.Models;

namespace ExpenseTracker.ViewModel
{
    public class HomeViewModel
    {
        public string FullName { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; } = [];
        public List<int> IncomeAmount { get; set; } = [];
        public List<int> ExpenseAmount { get; set; } = [];
        public List<string> PieLabels { get; set; } = [];
        public List<int> PieAmounts { get; set; } = [];
        public List<string> LineLabels { get; set; } = [];

        public int TotalIncome { get { return IncomeAmount.Sum(); } }
        public int TotalExpense { get { return ExpenseAmount.Sum(); } }
        public int TotalBalance { get { return (TotalIncome - TotalExpense); } }
    }
}
