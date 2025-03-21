using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Models;
namespace Informatics.MauiSurdeg.Interfaces
{
    public interface IRecipeService
    {
        //Profile 
        Task<IEnumerable<Recipe>> GetAllRecipesAsync();
        Task<bool> DeleteRecipeByNoAsync(string recipeNo);


        //Profile -> hämta alla recept 
        Task<List<Recipe>> GetRecipesByUserIdAsync(int userId);
        Task<Recipe?> GetRecipeByIdAsync(int id);
        Task<Recipe?> GetRecipeByNoAsync(string recipeNo);
        Task<Recipe> SaveRecipeAsync(Recipe recipe);
        Task<bool> DeleteRecipeByIdAsync(int id);
        Task<Recipe> AddNewRecipeAsync(Recipe newRecipe);

        // Generate Recipe
        Task<List<Recipe>> GetLatestRecipesAsync(int count);
    }
}