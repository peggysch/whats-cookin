using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;
using Informatics.MauiSurdeg.Services;

namespace Informatics.MauiSurdeg.ViewModels;

public partial class FeedViewModel : BaseViewModel
{
    private readonly IRecipeService _recipeService;

    // Observable collection to hold the recipes
    [ObservableProperty]
    private ObservableCollection<Recipe> _recipes = new();

    public FeedViewModel(IRecipeService recipeService)
    {
        _recipeService = recipeService;
        LoadRecipesCommand = new AsyncRelayCommand(LoadRecipesAsync);
    }

    public ICommand LoadRecipesCommand { get; }

    // Method to fetch recipes from the database
    private async Task LoadRecipesAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var recipesFromDb = await _recipeService.GetAllRecipesAsync();
            Recipes.Clear();

            foreach (var recipe in recipesFromDb)
            {
                Recipes.Add(recipe);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching recipes: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}

