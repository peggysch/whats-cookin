using System;
using Informatics.MauiSurdeg.Contexts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;
using Microsoft.EntityFrameworkCore;


namespace Informatics.MauiSurdeg.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly UniversityContext _context;

        public RecipeService(UniversityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            return await _context.Recipes
                .AsNoTracking()
                .Include(r => r.AppUser) // Include related user data
                .OrderByDescending(r => r.CreatedAt) // Sort by newest first
                .ToListAsync();
        }


        public async Task<Recipe?> GetRecipeByIdAsync(int id)
        {
            return await _context.Recipes
                .AsNoTracking()
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient) // hämta alla relationer med mängded av varje ingrediens 
                .Include(r => r.AppUser) // ladda alla ingridienser 
                .FirstOrDefaultAsync(r => r.RecipeID == id);
        }

        public async Task<Recipe?> GetRecipeByNoAsync(string recipeNo)
        {
            return await _context.Recipes
                .AsNoTracking()
                .Include(r => r.AppUser)
                .FirstOrDefaultAsync(r => r.RecipeNo == recipeNo);
        }

        public async Task<Recipe> SaveRecipeAsync(Recipe recipe)
        {
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));

            try
            {
                if (recipe.RecipeID > 0) // Update existing recipe
                {
                    
                    _context.Recipes.Update(recipe);
                }
                else // New recipe
                {
                    await _context.Recipes.AddAsync(recipe);
                }

                await _context.SaveChangesAsync();
                return recipe;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error saving recipe.", ex);
            }
        }
        public async Task<bool> DeleteRecipeByIdAsync(int id) //KOLLA
        {
            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(r => r.RecipeID == id);

            if (recipe == null) return false;

            try
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error deleting recipe.", ex);
            }
        }

        public async Task<bool> DeleteRecipeByNoAsync(string recipeNo) //KOLLA
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeNo == recipeNo);
            if (recipe == null) return false;

            try
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error deleting recipe.", ex);
            }
        }

        public async Task<List<Recipe>> GetRecipesByUserIdAsync(int userId)
        {
            return await _context.Recipes
                .Where(r => r.AppUserID == userId)
                .ToListAsync();
        }

        public async Task<Recipe> AddNewRecipeAsync(Recipe newRecipe)
        {
            // Generate new RecipeNo
            newRecipe.RecipeNo = await GenerateNextRecipeNoAsync();

            return await SaveRecipeAsync(newRecipe); // Save to database
        }

        private async Task<string> GenerateNextRecipeNoAsync()
        {
            var latestRecipe = await _context.Recipes
                .OrderByDescending(r => r.RecipeNo) // Senaste först
                .FirstOrDefaultAsync();

            if (latestRecipe == null || string.IsNullOrEmpty(latestRecipe.RecipeNo))
                return "R001"; // First user

            // Extract numeric part from "UXXX"
            string latestNumberStr = latestRecipe.RecipeNo.Substring(1); // Remove 'U'
            if (int.TryParse(latestNumberStr, out int latestNumber))
            {
                return $"R{(latestNumber + 1):D3}"; // Format as UXXX
            }

            throw new InvalidOperationException("Failed to generate RecipeNo.");
        }

    }
}