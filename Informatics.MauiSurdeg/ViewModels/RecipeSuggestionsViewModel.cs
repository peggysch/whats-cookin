using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Informatics.MauiSurdeg.Services;
using Informatics.MauiSurdeg.Models;
using Microsoft.Maui.Controls;
using Informatics.MauiSurdeg.Interfaces;

namespace Informatics.MauiSurdeg.ViewModels
{
    public class RecipeSuggestionsViewModel : BaseViewModel
    {
        private readonly IRecipeSuggestionService _recipeSuggestionService;

        public ObservableCollection<GeneratedRecipe> Recipes { get; private set; } = new();

        public ICommand GeneratePersonalizedCommand { get; }
        public ICommand GenerateCommunityCommand { get; }
        public ICommand GenerateUniqueCommand { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public RecipeSuggestionsViewModel(IRecipeSuggestionService recipeSuggestionService)
        {
            _recipeSuggestionService = recipeSuggestionService;

            GeneratePersonalizedCommand = new Command(async () => await GenerateRecipes("personalized"));
            GenerateCommunityCommand = new Command(async () => await GenerateRecipes("community"));
            GenerateUniqueCommand = new Command(async () => await GenerateRecipes("unique"));
        }

        private async Task GenerateRecipes(string type)
        {
            if (IsLoading) return; // Prevent duplicate clicks
            IsLoading = true;

            try
            {
                var result = await _recipeSuggestionService.GenerateRecipeSuggestions(type, App.UserId);
                
                if (result == null || result.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("No Recipes", "No suggestions were generated. Try again!", "OK");
                }

                Recipes = new ObservableCollection<GeneratedRecipe>(result);
                OnPropertyChanged(nameof(Recipes));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error fetching recipes: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to get recipe suggestions. Please try again later.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void ClearData()
        {
            Recipes.Clear();
            OnPropertyChanged(nameof(Recipes));
        }
    }
}
