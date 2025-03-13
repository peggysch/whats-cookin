using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Models;
namespace Informatics.MauiSurdeg.Interfaces
{
    public interface IRecipeService
    {
        //Profile 
        Task<IEnumerable<Recipe>> GetRecipesAsync();
        Task<bool> DeleteRecipeByNoAsync(string recipeNo);


        //Profile -> hämta alla recept 
        Task<List<Recipe>> GetRecipesByUserIdAsync(int userId);

    }
}