using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Models;

namespace Informatics.MauiSurdeg.Interfaces
{
    public interface IRecipeSuggestionService
    {
        Task<List<GeneratedRecipe>> GenerateRecipeSuggestions(string type, int userId);
    }
}

