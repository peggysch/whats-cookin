using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;
using Informatics.MauiSurdeg.ViewModels;

namespace Informatics.MauiSurdeg.Pages
{
    [QueryProperty(nameof(UserId), "userId")]
    public partial class ProfilePage : ContentPage
    {
        private readonly ProfileViewModel _viewModel;

        private int _userId;
        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged();
                LoadUserDataAsync(_userId); // Properly handling async
            }
        }

        public ProfilePage(ProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        private async void LoadUserDataAsync(int userId)
        {
            await _viewModel.SetUserIdAsync(userId);
        }
    }
}
