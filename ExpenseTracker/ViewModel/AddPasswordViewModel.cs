using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.ViewModel
{
    public class AddPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirm Password must match")]
        public string ConfirmNewPassword { get; set; }
    }
}
