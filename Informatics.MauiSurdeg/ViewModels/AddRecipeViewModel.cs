using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Informatics.MauiSurdeg.Models;
using Informatics.MauiSurdeg.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Informatics.MauiSurdeg.Interfaces;
using System.Threading.Tasks;

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
        AddIngredientCommand = new AsyncRelayCommand(AddIngredient);
        SaveRecipeCommand = new AsyncRelayCommand(SaveRecipeAsync);
        LoadIngredientsAsync();
        SelectedIngredients = new ObservableCollection<RecipeIngredient>();
    }

    private async void LoadIngredientsAsync()
    {
        var ingredients = await _ingredientService.GetIngredientsAsync();
        foreach (var ingredient in ingredients)
        {
            AvailableIngredients.Add(ingredient);
        }
    }

    //add error handling
    private async Task AddIngredient()
    {
        if (SelectedIngredient == null || string.IsNullOrWhiteSpace(IngredientQuantity))
            return;

        var existingIngredient = SelectedIngredients
        .FirstOrDefault(i => i.IngredientID == SelectedIngredient.IngredientID);

        if(existingIngredient != null)
        {
            bool replace = await Shell.Current.DisplayAlert(
                "Duplicate Ingredient",
                $"You have already added {SelectedIngredient.IngredientName}. Do you want to replace it?",
                "Yes", "No"
            );

            if(!replace)
            {
                IngredientQuantity = string.Empty;
                SelectedIngredient = null;
                return;
            }
            SelectedIngredients.Remove(existingIngredient);
        }

        var newRecipeIngredient = new RecipeIngredient
        {
            IngredientID = SelectedIngredient.IngredientID,
            Quantity = IngredientQuantity,
            Ingredient = SelectedIngredient
        };

        SelectedIngredients.Add(newRecipeIngredient);


        // Clear the input fields
        IngredientQuantity = string.Empty;
        SelectedIngredient = null;

        OnPropertyChanged(nameof(IngredientQuantity));
        OnPropertyChanged(nameof(SelectedIngredient));
    }

    //add error handling
    private async Task SaveRecipeAsync()
    {
        // Validate required fields before proceeding
        if (string.IsNullOrWhiteSpace(RecipeName) ||
            string.IsNullOrWhiteSpace(RecipeDescription) ||
            CookingTime <= 0 ||
            string.IsNullOrWhiteSpace(RecipeInstructions) ||
            RecipeCost <= 0 ||
            SelectedIngredients == null || SelectedIngredients.Count < 2)
        {
            await Shell.Current.DisplayAlert("Missing Information",
                "Please fill in all required fields and add at least two ingredients before adding the recipe", "OK");
            return;
        }

        try
        {
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

            var savedRecipe = await _recipeService.AddNewRecipeAsync(newRecipe);

            if (savedRecipe == null || savedRecipe.RecipeID == 0)
            {
                await Shell.Current.DisplayAlert("Error", "Recipe could not be saved. Please try again.", "OK");
                return;
            }

            // Save ingredients if any were selected
            foreach (var selectedIngredient in SelectedIngredients)
            {
                var recipeIngredient = new RecipeIngredient
                {
                    RecipeID = savedRecipe.RecipeID,
                    IngredientID = selectedIngredient.IngredientID,
                    Quantity = selectedIngredient.Quantity
                };

                await _recipeIngredientService.AddRecipeIngredientAsync(recipeIngredient);
            }

            await Shell.Current.DisplayAlert("Success", "Recipe created successfully!", "OK");

            // rensar all fält med clearData metoden 
            ClearData();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving recipe: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Something went wrong. Please try again later.", "OK");
        }
    }


    public void ClearData()
    {
        RecipeName = string.Empty;
        RecipeDescription = string.Empty;
        CookingTime = 0;
        RecipeInstructions = string.Empty;
        RecipeCost = 0;
        SelectedIngredient = null;
        IngredientQuantity = string.Empty;
        SelectedIngredients?.Clear();

        // Notify UI
        OnPropertyChanged(nameof(RecipeName));
        OnPropertyChanged(nameof(RecipeDescription));
        OnPropertyChanged(nameof(CookingTime));
        OnPropertyChanged(nameof(RecipeInstructions));
        OnPropertyChanged(nameof(RecipeCost));
        OnPropertyChanged(nameof(SelectedIngredient));
        OnPropertyChanged(nameof(IngredientQuantity));
        OnPropertyChanged(nameof(SelectedIngredients));

    }
}