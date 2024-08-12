using Microsoft.AspNetCore.Mvc;
using RecipeBook.Repository;

namespace RecipeBook.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IRecipeRepository _recipeRepository;
        public RecipeController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public IActionResult Index()
        {
            var recipes = _recipeRepository.GetAllRecipe();
            return View(recipes);
        }

        public IActionResult View(int id)
        {
            var recipe = _recipeRepository.GetRecipeById(id);
            return View(recipe);
        }
    }
}
