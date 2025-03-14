using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace Informatics.MauiSurdeg.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly IRecipeService _recipeService;

    [ObservableProperty]
    private ObservableCollection<Recipe> userRecipes;

    [ObservableProperty]
    private Recipe selectedRecipe;

    public ICommand LoadUserProfileCommand { get; }
    public ICommand SelectRecipeCommand { get; }
    public ICommand EditRecipeCommand { get; }
    public ICommand DeleteRecipeCommand { get; }

    public ProfileViewModel(IRecipeService recipeService)
    {
        _recipeService = recipeService ?? throw new ArgumentNullException(nameof(recipeService));

        UserRecipes = new ObservableCollection<Recipe>();

        LoadUserProfileCommand = new AsyncRelayCommand(LoadUserProfileAsync);
        SelectRecipeCommand = new AsyncRelayCommand<int>(SelectRecipeAsync);
        EditRecipeCommand = new AsyncRelayCommand(EditRecipeAsync);
        DeleteRecipeCommand = new AsyncRelayCommand(DeleteRecipeAsync);

        /* Force update of recipes if new recipe is added in recipe add page. Could be of good use later.
        Shell.Current.Navigated += async (sender, args) =>
        {
            if (args.Current.Location.ToString().Contains("profile"))
            {
                await LoadUserProfileAsync();
            }
        };*/
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

    private async Task SelectRecipeAsync(int recipeId)
    {
        // Implement logic for selecting a recipe
    }

    private async Task EditRecipeAsync()
    {
        // Implement logic for editing a recipe
    }

    private async Task DeleteRecipeAsync()
    {
        // Implement logic for deleting a recipe
    }
}
