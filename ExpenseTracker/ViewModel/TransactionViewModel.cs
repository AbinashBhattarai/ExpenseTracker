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
