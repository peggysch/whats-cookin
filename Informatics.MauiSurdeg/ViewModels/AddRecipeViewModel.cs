using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Informatics.MauiSurdeg.Models;
using Informatics.MauiSurdeg.Services;

namespace Informatics.MauiSurdeg.ViewModels;

public partial class AddRecipeViewModel : BaseViewModel
{
    private readonly IngredientService _ingredientService;

    public AddRecipeViewModel(IngredientService ingredientService)
    {
        _ingredientService = ingredientService;
        LoadIngredients();
        SelectedIngredients = new ObservableCollection<RecipeIngredient>();
        AddIngredientCommand = new Command(AddIngredient);

    }

    [Required]
    public string RecipeName { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public int Time { get; set; }
    public decimal CostPerPortion { get; set; }
    public Ingredient SelectedIngredient { get; set; }
    public string IngredientQuantity { get; set; }

    public ICommand AddIngredientCommand { get; }


    public ObservableCollection<Ingredient> AvailableIngredients { get; set; } = new();

    public ObservableCollection<RecipeIngredient> SelectedIngredients { get; set; }

    private async void LoadIngredients()
    {
        var ingredients = await _ingredientService.GetIngredientsAsync();
        foreach (var ingredient in ingredients)
        {
            AvailableIngredients.Add(ingredient);
        }
    }

    private void AddIngredient()
    {
        //Ensure selected ingredient is not null
        if (SelectedIngredient == null)
        {
            return; //exit the method to prevent errors
        }
        if (SelectedIngredient == null && !string.IsNullOrWhiteSpace(IngredientQuantity))
        {
            SelectedIngredients.Add(new RecipeIngredient
            {
                IngredientID = SelectedIngredient.IngredientID,
                Ingredient = SelectedIngredient,
                Quantity = IngredientQuantity
            });

            //Reset input fields
            IngredientQuantity = string.Empty;
            OnPropertyChanged(nameof(IngredientQuantity));
        }
    }
}
