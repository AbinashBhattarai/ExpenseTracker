using ExpenseTracker.Enum;
using ExpenseTracker.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ExpenseTracker.ViewModel
{
    public class CreateTransactionViewModel
    {
        [DisplayName("Transaction Date")]
        [Required(ErrorMessage = "Transaction date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Type can't be empty")]
        public TransactionType Type { get; set; }

        [MaxLength(50, ErrorMessage = "Note too long, can't exceed 50 characters")]
        public string? Note { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0, Double.MaxValue, ErrorMessage = "Quantity must be between 0 and 10")]
        public decimal Amount { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
        public List<Category>? Categories { get; set; }

        [ValidateNever]
        public DateTime MaxDate { get { return DateTime.Now; } }
    }


    public class EditTransactionViewModel : CreateTransactionViewModel
    {
        public int Id { get; set; }
    }
}
