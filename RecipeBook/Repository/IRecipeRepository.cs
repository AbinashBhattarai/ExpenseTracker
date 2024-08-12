using RecipeBook.Models;

namespace RecipeBook.Repository
{
    public interface IRecipeRepository
    {
        IEnumerable<Recipe> GetAllRecipe();
        Recipe GetRecipeById(int id);
    }
}
