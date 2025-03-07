using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Models;
namespace Informatics.MauiSurdeg.Interfaces

{
    public interface IIngredientService
    {
        Task<IEnumerable<Ingredient>> GetIngredientsAsync();
        Task<Ingredient?> GetIngredientByIdAsync(int id);
        Task<Ingredient?> GetIngredientByNoAsync(string ingredientNo);
        Task<Ingredient> SaveIngredientAsync(Ingredient ingredient);
        Task<bool> DeleteIngredientByIdAsync(int id);
        Task<bool> DeleteIngredientByNoAsync(string ingredientNo);
    }
}