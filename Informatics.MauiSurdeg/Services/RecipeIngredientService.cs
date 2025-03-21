using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Informatics.MauiSurdeg.Contexts;
using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;


namespace Informatics.MauiSurdeg.Services
{

    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly UniversityContext _context;

        public RecipeIngredientService(UniversityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<RecipeIngredient>> GetRecipeIngredientsAsync(int recipeId)
        {
            return await _context.RecipeIngredients
                .AsNoTracking()
                .Where(ri => ri.RecipeID == recipeId)
                .Include(ri => ri.Ingredient)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsForRecipeAsync(int recipeId)
        {
            return await _context.RecipeIngredients
                .AsNoTracking()
                .Where(ri => ri.RecipeID == recipeId)
                .Select(ri => ri.Ingredient)
                .ToListAsync();
        }

        public async Task<IEnumerable<Recipe>> GetRecipesForIngredientAsync(int ingredientId)
        {
            return await _context.RecipeIngredients
                .AsNoTracking()
                .Where(ri => ri.IngredientID == ingredientId)
                .Select(ri => ri.Recipe)
                .ToListAsync();
        }

        public async Task<bool> AddIngredientToRecipeAsync(int recipeId, int ingredientId, string quantity)
        {
            try
            {
                var recipeIngredient = new RecipeIngredient
                {
                    RecipeID = recipeId,
                    IngredientID = ingredientId,
                    Quantity = quantity
                };

                await _context.RecipeIngredients.AddAsync(recipeIngredient);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error adding ingredient to recipe.", ex);
            }
        }

        public async Task<bool> RemoveIngredientFromRecipeAsync(int recipeId, int ingredientId)
        {
            var recipeIngredient = await _context.RecipeIngredients
                .FirstOrDefaultAsync(ri => ri.RecipeID == recipeId && ri.IngredientID == ingredientId);
            if (recipeIngredient == null) return false;

            try
            {
                _context.RecipeIngredients.Remove(recipeIngredient);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error removing ingredient from recipe.", ex);
            }
        }

        public async Task<RecipeIngredient> AddRecipeIngredientAsync(RecipeIngredient newRecipeIngredient)
        {
            if (newRecipeIngredient == null)
            {
                throw new ArgumentNullException(nameof(newRecipeIngredient), "RecipeIngredient cannot be null.");
            }

            try
            {
                await _context.RecipeIngredients.AddAsync(newRecipeIngredient);
                await _context.SaveChangesAsync();
                return newRecipeIngredient; // Return the saved entity with assigned ID
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error saving RecipeIngredient to the database.", ex);
            }
        }

        public async Task<RecipeIngredient?> UpdateRecipeIngredientAsync(RecipeIngredient updatedRecipeIngredient)
        {
            var existing = await _context.RecipeIngredients
            .FirstOrDefaultAsync(ri => ri.RecipeID == updatedRecipeIngredient.RecipeID && ri.IngredientID == updatedRecipeIngredient.IngredientID);

            if (existing != null)
            {
                existing.Quantity = updatedRecipeIngredient.Quantity;
                await _context.SaveChangesAsync();
                return updatedRecipeIngredient;
            }
            return null;
        }


    }
}
