namespace Informatics.MauiSurdeg.Pages;
using Informatics.MauiSurdeg.ViewModels;
using Informatics.MauiSurdeg.Pages;

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
        await _viewModel.LoadUserProfileAsync();
    }
    // private void OnPasswordTextChanged(object sender, TextChangedEventArgs e)
    // {
    //     var viewModel = BindingContext as SettingsViewModel;
    //     if (viewModel != null)
    //     {
    //         viewModel.CanExecuteSavePassword = !string.IsNullOrWhiteSpace(viewModel.Password)
    //                                         && viewModel.Password.Length >= 5
    //                                         && !string.IsNullOrWhiteSpace(viewModel.ConfirmPassword)
    //                                         && viewModel.Password == viewModel.ConfirmPassword;
    //     }
    // }
}