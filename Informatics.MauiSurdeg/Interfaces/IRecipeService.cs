using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Models;
namespace Informatics.MauiSurdeg.Interfaces
{
    public interface IRecipeService
    {
        Task<IEnumerable<Recipe>> GetRecipesAsync();
        Task<Recipe?> GetRecipeByIdAsync(int id);
        Task<Recipe?> GetRecipeByNoAsync(string recipeNo);
        Task<Recipe> SaveRecipeAsync(Recipe recipe);
        Task<bool> DeleteRecipeByIdAsync(int id);
        Task<bool> DeleteRecipeByNoAsync(string recipeNo);
    }
}