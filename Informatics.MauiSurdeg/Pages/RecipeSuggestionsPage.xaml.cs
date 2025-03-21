using Microsoft.Maui.Controls;
using Informatics.MauiSurdeg.ViewModels;

namespace Informatics.MauiSurdeg.Pages
{
    public partial class RecipeSuggestionsPage : ContentPage
    {
        private readonly RecipeSuggestionsViewModel _viewModel;

        public RecipeSuggestionsPage(RecipeSuggestionsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

    }
}
