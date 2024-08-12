using RecipeBook.Models;

namespace RecipeBook.Repository
{
    public class MockRecipeRepository : IRecipeRepository
    {
        private List<Recipe> _recipes;
        public MockRecipeRepository()
        {
            _recipes = new List<Recipe>()
            {
                new Recipe()
                {
                    Id = 1,
                    Name = "Chicken Chilly",
                    Description = "Pickles are the perfect side dish to any meal!Try out these authentic and flavoursome pickle recipes to add spice to your meals!",
                    Image = "https://images.pexels.com/photos/1640777/pexels-photo-1640777.jpeg?auto=compress&cs=tinysrgb&w=600",
                    Category = "Vegetarian",
                    PostedOn = DateTime.Now,
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient()
                        {
                            Id = 1,
                            Quantity = 2,
                            Unit = "kg",
                            Item = "Chicken",
                            RecipeId = 1
                        },
                        new Ingredient()
                        {
                            Id = 2,
                            Quantity = 3,
                            Unit = "tbsp",
                            Item = "salt",
                            RecipeId = 1
                        },
                        new Ingredient()
                        {
                            Id = 3,
                            Quantity = 2,
                            Unit = "tbsp",
                            Item = "masala",
                            RecipeId = 1
                        }
                    },
                    Instructions = new List<Instruction>()
                    {
                        new Instruction()
                        {
                            Id = 1,
                            Direction = "Pack the whole chicken",
                            RecipeId = 1
                        },
                        new Instruction()
                        {
                            Id = 2,
                            Direction = "Put salt",
                            RecipeId = 1
                        },
                        new Instruction()
                        {
                            Id = 3,
                            Direction = "Put masala",
                            RecipeId = 1
                        }
                    }
                },
                new Recipe()
                {
                    Id = 2,
                    Name = "Mutton Chilly",
                    Description = "Pickles are the perfect side dish to any meal!Try out these authentic and flavoursome pickle recipes to add spice to your meals!",
                    Image = "https://images.pexels.com/photos/1099680/pexels-photo-1099680.jpeg?auto=compress&cs=tinysrgb&w=600",
                    Category = "Vegetarian",
                    PostedOn = DateTime.Now,
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient()
                        {
                            Id = 4,
                            Quantity = 2,
                            Unit = "kg",
                            Item = "Chicken",
                            RecipeId = 2
                        },
                        new Ingredient()
                        {
                            Id = 5,
                            Quantity = 3,
                            Unit = "tbsp",
                            Item = "salt",
                            RecipeId = 2
                        },
                        new Ingredient()
                        {
                            Id = 6,
                            Quantity = 2,
                            Unit = "tbsp",
                            Item = "masala",
                            RecipeId = 2
                        }
                    },
                    Instructions = new List<Instruction>()
                    {
                        new Instruction()
                        {
                            Id = 4,
                            Direction = "Pack the whole chicken",
                            RecipeId = 2
                        },
                        new Instruction()
                        {
                            Id = 5,
                            Direction = "Put salt",
                            RecipeId = 2
                        },
                        new Instruction()
                        {
                            Id = 6,
                            Direction = "Put masala",
                            RecipeId = 2
                        }
                    }
                },
                new Recipe()
                {
                    Id = 3,
                    Name = "Fish Curry",
                    Description = "Pickles are the perfect side dish to any meal!Try out these authentic and flavoursome pickle recipes to add spice to your meals!",
                    Image = "https://images.pexels.com/photos/1640772/pexels-photo-1640772.jpeg?auto=compress&cs=tinysrgb&w=600",
                    Category = "Vegetarian",
                    PostedOn = DateTime.Now,
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient()
                        {
                            Id = 7,
                            Quantity = 2,
                            Unit = "kg",
                            Item = "Chicken",
                            RecipeId = 3
                        },
                        new Ingredient()
                        {
                            Id = 8,
                            Quantity = 3,
                            Unit = "tbsp",
                            Item = "salt",
                            RecipeId = 3
                        },
                        new Ingredient()
                        {
                            Id = 9,
                            Quantity = 2,
                            Unit = "tbsp",
                            Item = "masala",
                            RecipeId = 3
                        }
                    },
                    Instructions = new List<Instruction>()
                    {
                        new Instruction()
                        {
                            Id = 7,
                            Direction = "Pack the whole chicken",
                            RecipeId = 3
                        },
                        new Instruction()
                        {
                            Id = 8,
                            Direction = "Put salt",
                            RecipeId = 3
                        },
                        new Instruction()
                        {
                            Id = 9,
                            Direction = "Put masala",
                            RecipeId = 3
                        }
                    }
                },
                new Recipe()
                {
                    Id = 4,
                    Name = "Gobi Masala",
                    Description = "Pickles are the perfect side dish to any meal!Try out these authentic and flavoursome pickle recipes to add spice to your meals!",
                    Image = "https://images.pexels.com/photos/699953/pexels-photo-699953.jpeg?auto=compress&cs=tinysrgb&w=600",
                    Category = "Vegetarian",
                    PostedOn = DateTime.Now,
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient()
                        {
                            Id = 10,
                            Quantity = 2,
                            Unit = "kg",
                            Item = "Chicken",
                            RecipeId = 4
                        },
                        new Ingredient()
                        {
                            Id = 11,
                            Quantity = 3,
                            Unit = "tbsp",
                            Item = "salt",
                            RecipeId = 4
                        },
                        new Ingredient()
                        {
                            Id = 12,
                            Quantity = 2,
                            Unit = "tbsp",
                            Item = "masala",
                            RecipeId = 4
                        }
                    },
                    Instructions = new List<Instruction>()
                    {
                        new Instruction()
                        {
                            Id = 10,
                            Direction = "Pack the whole chicken",
                            RecipeId = 4
                        },
                        new Instruction()
                        {
                            Id = 11,
                            Direction = "Put salt",
                            RecipeId = 4
                        },
                        new Instruction()
                        {
                            Id = 12,
                            Direction = "Put masala",
                            RecipeId = 4
                        }
                    }
                }
            };
        }
        public IEnumerable<Recipe> GetAllRecipe()
        {
            return _recipes;
        }

        public Recipe GetRecipeById(int id)
        {
            return _recipes.FirstOrDefault(r => r.Id == id);
        }
    }
}
