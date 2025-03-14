using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;
using Informatics.MauiSurdeg.ViewModels;

namespace Informatics.MauiSurdeg.Pages
{
    public partial class ProfilePage : ContentPage
    {
        private readonly ProfileViewModel _viewModel;

        public ProfilePage(ProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadUserProfileAsync(); // Load recipes for a specific users
        }
    }
}
