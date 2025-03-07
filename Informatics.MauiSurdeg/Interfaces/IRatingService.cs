using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Models;
namespace Informatics.MauiSurdeg.Interfaces

{
    public interface IRatingService
    {
        Task<IEnumerable<Rating>> GetRatingsAsync();
        Task<Rating?> GetRatingByIdAsync(int id);
        // Task<Rating?> GetRatingByNoAsync(string ratingNo);
        Task<Rating?> GetUserRatingForRecipeAsync(int userId, int recipeId);
        Task<Rating> SaveRatingAsync(Rating rating);
        Task<bool> DeleteRatingByIdAsync(int id);
        // Task<bool> DeleteRatingByNoAsync(string ratingNo);
    }
}
