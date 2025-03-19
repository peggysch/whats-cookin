using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Models;
namespace Informatics.MauiSurdeg.Interfaces
{
    public interface IRecipeIngredientService
    {
        Task<IEnumerable<RecipeIngredient>> GetRecipeIngredientsAsync(int recipeId);
        
        Task<IEnumerable<Ingredient>> GetIngredientsForRecipeAsync(int recipeId);
        Task<IEnumerable<Recipe>> GetRecipesForIngredientAsync(int ingredientId);
        Task<bool> AddIngredientToRecipeAsync(int recipeId, int ingredientId, string quantity);
        Task<bool> RemoveIngredientFromRecipeAsync(int recipeId, int ingredientId);
        Task<RecipeIngredient> AddRecipeIngredientAsync(RecipeIngredient newRecipeIngredient);
        Task<RecipeIngredient?> UpdateRecipeIngredientAsync(RecipeIngredient updatedRecipeIngredient);
    }
}
