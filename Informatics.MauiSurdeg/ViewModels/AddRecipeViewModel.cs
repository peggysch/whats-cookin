using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Informatics.MauiSurdeg.Models;
using Informatics.MauiSurdeg.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Informatics.MauiSurdeg.Interfaces;

namespace Informatics.MauiSurdeg.ViewModels;

public partial class AddRecipeViewModel : BaseViewModel
{
    [ObservableProperty]
    private string recipeName;

    [ObservableProperty]
    private string recipeDescription;

    [ObservableProperty]
    private int cookingTime;

    [ObservableProperty]
    private string recipeInstructions;

    [ObservableProperty]
    private int recipeCost;

    private readonly IIngredientService _ingredientService;
    private readonly IRecipeService _recipeService;
    private readonly IRecipeIngredientService _recipeIngredientService;

    public ICommand AddIngredientCommand { get; }
    public ICommand SaveRecipeCommand { get; }

    public ObservableCollection<Ingredient> AvailableIngredients { get; set; } = new();
    public ObservableCollection<RecipeIngredient> SelectedIngredients { get; set; } = new();

    [ObservableProperty]
    private Ingredient? selectedIngredient;

    [ObservableProperty]
    private string ingredientQuantity = string.Empty;

    public AddRecipeViewModel(IIngredientService ingredientService, IRecipeService recipeService, IRecipeIngredientService recipeIngredientService)
    {
        _ingredientService = ingredientService;
        _recipeService = recipeService;
        _recipeIngredientService = recipeIngredientService;
        AddIngredientCommand = new RelayCommand(AddIngredient);
        SaveRecipeCommand = new AsyncRelayCommand(SaveRecipe);
        LoadIngredients();
        SelectedIngredients = new ObservableCollection<RecipeIngredient>();
    }

    // Computed property for displaying selected ingredients with names
    /*public IEnumerable<(string IngredientName, string Quantity)> DisplayedIngredients
    {
        get
        {
            return SelectedIngredients.Select(ri =>
            {
                var ingredient = AvailableIngredients.FirstOrDefault(i => i.IngredientID == ri.IngredientID);
                return (ingredient?.IngredientName ?? "Unknown", ri.Quantity);
            });
        }
    }*/

    private async void LoadIngredients()
    {
        var ingredients = await _ingredientService.GetIngredientsAsync();
        foreach (var ingredient in ingredients)
        {
            AvailableIngredients.Add(ingredient);
        }
    }

    //add error handling
    private void AddIngredient()
    {
        if (SelectedIngredient == null || string.IsNullOrWhiteSpace(IngredientQuantity))
            return;

        var newRecipeIngredient = new RecipeIngredient
        {
            IngredientID = SelectedIngredient.IngredientID,
            Quantity = IngredientQuantity,

        };

        SelectedIngredients.Add(newRecipeIngredient);


        // Clear the input fields
        IngredientQuantity = string.Empty;
        SelectedIngredient = null;

        OnPropertyChanged(nameof(IngredientQuantity));
        OnPropertyChanged(nameof(SelectedIngredient));
    }

    //add error handling
    private async Task SaveRecipe()
    {
        // Step 1: Create and save the Recipe
        var newRecipe = new Recipe
        {
            RecipeName = RecipeName,
            RecipeDescription = RecipeDescription,
            CookingTime = CookingTime,
            RecipeInstructions = RecipeInstructions,
            RecipeCost = RecipeCost,
            CreatedAt = DateTime.UtcNow,
            AppUserID = App.UserId // Get the current user ID
        };

        // Step 2: Save the recipe to the database and get the RecipeID
        var savedRecipe = await _recipeService.AddNewRecipeAsync(newRecipe);

        if (savedRecipe == null || savedRecipe.RecipeID == 0)
        {
            // Handle error: Recipe could not be saved
            Console.WriteLine("Error: Recipe could not be saved.");
            return;
        }

        // Step 3: Add RecipeIngredients if any were selected
        foreach (var selectedIngredient in SelectedIngredients)
        {
            var recipeIngredient = new RecipeIngredient
            {
                RecipeID = savedRecipe.RecipeID, // Use the saved RecipeID
                IngredientID = selectedIngredient.IngredientID,
                Quantity = selectedIngredient.Quantity
            };

            await _recipeIngredientService.AddRecipeIngredientAsync(recipeIngredient);
        }

        Console.WriteLine("Recipe and ingredients saved successfully!");
    }
}