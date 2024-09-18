using ExpenseTracker.Enum;
using ExpenseTracker.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ExpenseTracker.ViewModel
{
    public class TransactionViewModel
    {
        public IEnumerable<Transaction> Transactions { get; set; } = [];
        public string? SearchString { get; set; }
        public string? CategorySortParam { get; set; }
        public string? DateSortParam { get; set; }
        public string? AmountSortParam { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ResultMsg { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; } 
        public string? SortOrder { get; set; }
    }


    public class CreateTransactionViewModel
    {
        [DisplayName("Transaction Date")]
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Type is required")]
        public TransactionType Type { get; set; }

        [MaxLength(50, ErrorMessage = "Note too long, can't exceed 50 characters")]
        public string? Note { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
        public int Amount { get; set; }

        [DisplayName("Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }


    public class EditTransactionViewModel : CreateTransactionViewModel
    {
        public int Id { get; set; }
    }
}
