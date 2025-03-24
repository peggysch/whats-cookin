using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Specialized;
using Informatics.MauiSurdeg.Services;

namespace Informatics.MauiSurdeg.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly IRecipeService _recipeService;
    private readonly IIngredientService _ingredientService;

    private readonly IRecipeIngredientService _recipeingredientsService;

    [ObservableProperty]
    private ObservableCollection<Recipe> userRecipes;

    [ObservableProperty]
    private ObservableCollection<Ingredient> availableIngredients;

    [ObservableProperty]
    private ObservableCollection<RecipeIngredient> recipeIngredients;

    [ObservableProperty]
    private Recipe selectedRecipe;
    public bool IsRecipeSelected => SelectedRecipe != null;

    public Ingredient SelectedIngredient { get; set; }
    public string NewIngredientQuantity { get; set; }

    partial void OnSelectedRecipeChanged(Recipe value)
    {
        OnPropertyChanged(nameof(IsRecipeSelected));
        OnPropertyChanged(nameof(SelectedRecipe));

        if (value != null)
        {
            LoadRecipeIngredientsAsync(selectedRecipe.RecipeID);

            // Nollställ ingrediens-val och mängd vid receptbyte
            SelectedIngredient = null;
            NewIngredientQuantity = "";

            // Notifiera UI att dessa fält har ändrats
            OnPropertyChanged(nameof(SelectedIngredient));
            OnPropertyChanged(nameof(NewIngredientQuantity));
        }
    }
    public ICommand LoadUserProfileCommand { get; }
    public ICommand SelectRecipeCommand { get; }
    public ICommand EditRecipeCommand { get; }
    public ICommand DeleteRecipeCommand { get; }

    //Regigera ett recept 
    public ICommand AddIngredientCommand { get; }
    public ICommand RemoveIngredientCommand { get; }


    public ProfileViewModel(IRecipeService recipeService, IIngredientService ingredientService, IRecipeIngredientService recipeIngredientService)
    {
        _recipeService = recipeService ?? throw new ArgumentNullException(nameof(recipeService));
        _ingredientService = ingredientService ?? throw new ArgumentException(nameof(ingredientService));
        _recipeingredientsService = recipeIngredientService ?? throw new ArgumentException(nameof(recipeIngredientService));

        UserRecipes = new ObservableCollection<Recipe>();
        RecipeIngredients = new ObservableCollection<RecipeIngredient>();
        AvailableIngredients = new ObservableCollection<Ingredient>();

        LoadUserProfileCommand = new AsyncRelayCommand(LoadUserProfileAsync);
        EditRecipeCommand = new AsyncRelayCommand(EditRecipeAsync);
        DeleteRecipeCommand = new AsyncRelayCommand(DeleteRecipeAsync);

        AddIngredientCommand = new AsyncRelayCommand(AddIngredient);
        RemoveIngredientCommand = new AsyncRelayCommand<RecipeIngredient>(RemoveIngredientAsync);

        LoadAvailableIngredientsAsync();

    }

    public async Task LoadUserProfileAsync()
    {
        try
        {
            IsBusy = true;

            int userId = App.UserId; // Hämta UserId globalt från App
            if (userId > 0) // Kontrollera att UserId är giltigt
            {
                var recipes = await _recipeService.GetRecipesByUserIdAsync(userId);
                UserRecipes.Clear();
                foreach (var recipe in recipes)
                {
                    UserRecipes.Add(recipe);
                }

            }
            else
            {
                Console.WriteLine("Invalid UserId, cannot fetch recipes.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user profile: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    //hämta alla ingredienser 
    private async void LoadAvailableIngredientsAsync()
    {
        try
        {
            var ingredients = await _ingredientService.GetIngredientsAsync();
            AvailableIngredients.Clear();
            foreach (var ingredient in ingredients)
            {
                AvailableIngredients.Add(ingredient);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading ingredients: {ex.Message}");
        }
    }

    // hämtart alla ingredienser för ett recept 
    private async void LoadRecipeIngredientsAsync(int recipeId)
    {
        try
        {
            var recipeIngredients = await _recipeingredientsService.GetRecipeIngredientsAsync(recipeId);
            RecipeIngredients.Clear();

            foreach (var ingredient in recipeIngredients)
            {
                RecipeIngredients.Add(ingredient);
            }
            OnPropertyChanged(nameof(RecipeIngredients));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading recipe ingredients: {ex.Message}");
        }

    }

    public string CookingTimeString
    {
        get => SelectedRecipe?.CookingTime.ToString() ?? "";
        set
        {
            if (SelectedRecipe != null && int.TryParse(value, out int time))
            {
                SelectedRecipe.CookingTime = time;
                OnPropertyChanged(nameof(CookingTimeString));
            }
        }
    }

    public string RecipeCostString
    {
        get => SelectedRecipe?.RecipeCost.ToString() ?? "";
        set
        {
            if (SelectedRecipe != null && int.TryParse(value, out int cost))
            {
                SelectedRecipe.RecipeCost = cost;
                OnPropertyChanged(nameof(RecipeCostString));
            }
        }
    }

    public DateTime MaxDate => DateTime.Today;
    public DateTime CreatedAtDate
    {
        get => SelectedRecipe?.CreatedAt.Date ?? DateTime.Today;
        set
        {
            if (value > DateTime.Today)
                value = DateTime.Today;

            SelectedRecipe.CreatedAt = value + SelectedRecipe.CreatedAt.TimeOfDay;
            OnPropertyChanged(nameof(CreatedAtDate));

        }
    }
    private async Task EditRecipeAsync()
    {
        if (SelectedRecipe == null)
        {
            return;
        }

        bool isConfirmed = await Shell.Current.DisplayAlert(
            "Confirm Edit",
            "Are you sure you want to save the changes to this recipe?",
            "Yes",
            "No"
        );
        if (!isConfirmed)
        {
            return;
        }

        try
        {
            IsBusy = true;

            // Steg 1: Uppdatera receptet i databasen
            var updatedRecipe = await _recipeService.SaveRecipeAsync(SelectedRecipe);

            if (updatedRecipe == null)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to update recipe.", "OK");
                return;
            }

            // Uppdatera UI så att den senaste versionen av receptet visas
            var index = UserRecipes.IndexOf(SelectedRecipe);
            if (index >= 0)
            {
                UserRecipes[index] = updatedRecipe;
            }

            // Steg 2: Hämta nuvarande ingredienser från databasen
            var existingIngredients = await _recipeingredientsService.GetRecipeIngredientsAsync(SelectedRecipe.RecipeID);

            //steg 3: tar bort ingredines från recipe 
            foreach (var existingIngredient in existingIngredients)
            {
                var stillExists = RecipeIngredients.Any(i => i.Ingredient.IngredientID == existingIngredient.Ingredient.IngredientID);
                if (!stillExists)
                {
                    await _recipeingredientsService.RemoveIngredientFromRecipeAsync(SelectedRecipe.RecipeID, existingIngredient.IngredientID);
                }
            }

            // Steg 4: Lägg till eller uppdatera ingredienser
            foreach (var ingredient in RecipeIngredients)
            {
                var existing = existingIngredients.FirstOrDefault(i => i.Ingredient.IngredientID == ingredient.Ingredient.IngredientID);

                if (existing == null)
                {
                    // Om ingrediensen inte fanns tidigare, lägg till den
                    var newIngredient = new RecipeIngredient
                    {
                        RecipeID = SelectedRecipe.RecipeID,
                        IngredientID = ingredient.Ingredient.IngredientID,
                        Quantity = ingredient.Quantity
                    };
                    await _recipeingredientsService.AddRecipeIngredientAsync(newIngredient);
                }
                else if (existing.Quantity != ingredient.Quantity)
                {
                    existing.Quantity = ingredient.Quantity;
                    await _recipeingredientsService.UpdateRecipeIngredientAsync(existing);
                }
            }


            await Shell.Current.DisplayAlert("Success", "Recipe updated successfully!", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }



    private async Task AddIngredient()
    {
        if (SelectedIngredient == null || string.IsNullOrWhiteSpace(NewIngredientQuantity))
            return;

        var existingIngredient = RecipeIngredients.FirstOrDefault(i => i.Ingredient.IngredientID == SelectedIngredient.IngredientID);

        if (existingIngredient != null)
        {
            bool replaceIngredient = await Shell.Current.DisplayAlert(
                "Ingredient Exists",
                $"You already added {SelectedIngredient.IngredientName}. Do you want to replace it",
                "Yes",
                "No"
            );

            if (!replaceIngredient)
            {
                return;
            }
            // Tar bort den gamla och uppdatera mängden om ingrediensen redan finns
            RecipeIngredients.Remove(existingIngredient);
            var updatedIngredient = new RecipeIngredient
            {
                RecipeID = SelectedRecipe.RecipeID,
                Ingredient = SelectedIngredient,
                Quantity = NewIngredientQuantity
            };
            RecipeIngredients.Add(updatedIngredient);

        }
        else
        {
            // Lägg till den nya ingrediensen i listan
            var newRecipeIngredient = new RecipeIngredient
            {
                RecipeID = SelectedRecipe.RecipeID,
                Ingredient = SelectedIngredient,
                Quantity = NewIngredientQuantity
            };

            RecipeIngredients.Add(newRecipeIngredient);
        }

        // Rensa input-fälten
        SelectedIngredient = null;
        NewIngredientQuantity = "";

        // Uppdatera UI
        OnPropertyChanged(nameof(SelectedIngredient));
        OnPropertyChanged(nameof(NewIngredientQuantity));
        OnPropertyChanged(nameof(RecipeIngredients));
    }


    private Task RemoveIngredientAsync(RecipeIngredient ingredient)
    {
        if (SelectedRecipe == null || ingredient == null)
        {
            return Task.CompletedTask;
        }

        // Remove from UI
        RecipeIngredients.Remove(ingredient);


        // Notify UI update
        OnPropertyChanged(nameof(RecipeIngredients));

        return Task.CompletedTask;
    }


    public async Task DeleteRecipeAsync()
    {
        if (SelectedRecipe == null)
        {
            return; // Ingen selektion = inget att ta bort
        }

        bool isConfirmed = await Shell.Current.DisplayAlert("Confirm Delete",
            $"Are you sure you want to delete {SelectedRecipe.RecipeName}?", "Yes", "No");

        if (!isConfirmed)
        {
            return; // Om användaren avbryter raderingen, gör inget
        }

        try
        {
            IsBusy = true;

            bool isDeleted = await _recipeService.DeleteRecipeByIdAsync(SelectedRecipe.RecipeID);
            if (isDeleted)
            {
                UserRecipes.Remove(SelectedRecipe);
                SelectedRecipe = null;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Recipe could not be deleted", "Ok");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting recipe: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void ClearData()
    {
        SelectedRecipe = null;
        UserRecipes?.Clear();
        RecipeIngredients?.Clear();
        SelectedIngredient = null;

        OnPropertyChanged(nameof(SelectedRecipe));
        OnPropertyChanged(nameof(UserRecipes));
        OnPropertyChanged(nameof(RecipeIngredients));
        OnPropertyChanged(nameof(SelectedIngredient));
        OnPropertyChanged(nameof(NewIngredientQuantity));

    }

}
