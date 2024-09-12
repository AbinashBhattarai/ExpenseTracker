using ExpenseTracker.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public DateTime Date {  get; set; }
        public TransactionType Type { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Note { get; set; }

        public int Amount { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
