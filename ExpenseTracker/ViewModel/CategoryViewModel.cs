using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.ViewModel
{
    public class CreateCategoryViewModel
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Category name must be between 3 to 50 characters")]
        [Required(ErrorMessage ="Category is required")]
        [DisplayName("Category Name")]
        public string Name { get; set; }
    }

    public class EditCategoryViewModel : CreateCategoryViewModel
    {
        public int Id { get; set; }
    }
}
