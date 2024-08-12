using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RecipeBook.Models
{
    public class Instruction
    {
        public int Id { get; set; }
        public string Direction { get; set; }

        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}
