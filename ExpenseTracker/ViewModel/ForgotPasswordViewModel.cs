using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
