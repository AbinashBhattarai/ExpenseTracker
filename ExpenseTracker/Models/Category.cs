using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Column(TypeName ="nvarchar(50)")]
        public string Name { get; set; }


        public string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
