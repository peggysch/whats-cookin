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
    private readonly IIngredientService _ingredientService;
    private readonly IRecipeService _recipeService;
    private readonly IRecipeIngredientService _recipeIngredientService;

    public AddRecipeViewModel(IIngredientService ingredientService, IRecipeService recipeService, IRecipeIngredientService recipeIngredientService)
    {
        _ingredientService = ingredientService;
        _recipeService = recipeService;
        _recipeIngredientService = recipeIngredientService;
        LoadIngredients();
        SelectedIngredients = new ObservableCollection<RecipeIngredient>();
    }

    [ObservableProperty] 
    private string recipeName = string.Empty;

    [ObservableProperty] 
    private string recipeDescription = string.Empty;

    [ObservableProperty] 
    private string instructions = string.Empty;

    [ObservableProperty] 
    private int cookingTime;
    
    [ObservableProperty] 
    private decimal costPerPortion;

    public ObservableCollection<Ingredient> AvailableIngredients { get; set; } = new();
    public ObservableCollection<RecipeIngredient> SelectedIngredients { get; set; }

    [ObservableProperty] private Ingredient? selectedIngredient;
    [ObservableProperty] private string ingredientQuantity = string.Empty;

    [RelayCommand]
    private async void LoadIngredients()
    {
        var ingredients = await _ingredientService.GetIngredientsAsync();
        foreach (var ingredient in ingredients)
        {
            AvailableIngredients.Add(ingredient);
        }
    }

    [RelayCommand]
    private void AddIngredient()
    {
        if (SelectedIngredient == null || string.IsNullOrWhiteSpace(IngredientQuantity)) 
            return;
        
        SelectedIngredients.Add(new RecipeIngredient
        {
            Ingredient = SelectedIngredient,
            IngredientID = SelectedIngredient.IngredientID,
            Quantity = IngredientQuantity
        });

        IngredientQuantity = string.Empty;
    }

    [RelayCommand]
    private async void SaveRecipe()
    {
        var newRecipe = new Recipe
        {
            RecipeName = RecipeName,
            RecipeDescription = RecipeDescription,
            Instructions = Instructions,
            CookingTime = CookingTime,
            CostPerPortion = CostPerPortion,
            CreatedAt = DateTime.UtcNow,
            AppUserID = 1, // TODO: Get the current user ID
            RecipeIngredients = SelectedIngredients.ToList()

        };

        await _recipeService.SaveRecipeAsync(newRecipe);
    }

    


}