namespace Informatics.MauiSurdeg.Pages;
using Informatics.MauiSurdeg.ViewModels;

public partial class SettingsPage : ContentPage
{

 private readonly SettingsViewModel _viewModel;

        public SettingsPage(SettingsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }
}