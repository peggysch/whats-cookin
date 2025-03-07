using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Models;
using Informatics.MauiSurdeg.Contexts;
using Informatics.MauiSurdeg.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Informatics.MauiSurdeg.Services
{
    public class RatingService : IRatingService
    {
        private readonly UniversityContext _context;

        public RatingService(UniversityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Rating>> GetRatingsAsync()
        {
            return await _context.Ratings
                .AsNoTracking()
                .Include(r => r.AppUser)
                .Include(r => r.Recipe)
                .ToListAsync();
        }

        public async Task<Rating?> GetRatingByIdAsync(int id)
        {
            return await _context.Ratings
                .AsNoTracking()
                .Include(r => r.AppUser)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(r => r.RatingId == id);
        }

        public async Task<Rating?> GetUserRatingForRecipeAsync(int userId, int recipeId)
        {
            return await _context.Ratings
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.AppUserId == userId && r.RecipeId == recipeId);
        }

        public async Task<Rating> SaveRatingAsync(Rating rating)
        {
            if (rating == null) throw new ArgumentNullException(nameof(rating));

            try
            {
                if (rating.Id > 0) // Update existing rating
                {
                    _context.Ratings.Update(rating);
                }
                else // New rating
                {
                    await _context.Ratings.AddAsync(rating);
                }

                await _context.SaveChangesAsync();
                return rating;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error saving rating.", ex);
            }
        }

        public async Task<bool> DeleteRatingByIdAsync(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null) return false;

            try
            {
                _context.Ratings.Remove(rating);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error deleting rating.", ex);
            }
            public async 
        }
    }
}
