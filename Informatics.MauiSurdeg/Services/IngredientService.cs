using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Contexts;
using Informatics.MauiSurdeg.Models;
using Microsoft.EntityFrameworkCore;


namespace Informatics.MauiSurdeg.Services
{
public class IngredientService : IIngredientService
    {
        private readonly UniversityContext _context;

        public IngredientService(UniversityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsAsync()
        {
            return await _context.Ingredients
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Ingredient?> GetIngredientByIdAsync(int id)
        {
            return await _context.Ingredients
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Ingredient?> GetIngredientByNoAsync(string ingredientNo)
        {
            return await _context.Ingredients
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Number == ingredientNo);
        }

        public async Task<Ingredient> SaveIngredientAsync(Ingredient ingredient)
        {
            if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));

            try
            {
                if (ingredient.Id > 0) // Update existing ingredient
                {
                    _context.Ingredients.Update(ingredient);
                }
                else // New ingredient
                {
                    await _context.Ingredients.AddAsync(ingredient);
                }

                await _context.SaveChangesAsync();
                return ingredient;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error saving ingredient.", ex);
            }
        }

        public async Task<bool> DeleteIngredientByIdAsync(int id) //KOLLA
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null) return false;

            try
            {
                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error deleting ingredient.", ex);
            }
        }

        public async Task<bool> DeleteIngredientByNoAsync(string ingredientNo) //KOLLA
        {
            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Number == ingredientNo);
            if (ingredient == null) return false;

            try
            {
                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error deleting ingredient.", ex);
            }
        }
    }
}
