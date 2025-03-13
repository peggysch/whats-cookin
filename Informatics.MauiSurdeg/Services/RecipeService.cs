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

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            return await _context.Recipes
                .AsNoTracking()
                .Include(r => r.AppUser) // Include related user data
                .ToListAsync();
        }

        public async Task<Recipe?> GetRecipeByIdAsync(int id)
        {
            return await _context.Recipes
                .AsNoTracking()
                .Include(r => r.AppUser)
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
            var recipe = await _context.Recipes.FindAsync(id);
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
    }
}