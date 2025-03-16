using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Informatics.MauiSurdeg.Interfaces;
using System.Diagnostics;
namespace Informatics.MauiSurdeg.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    // Observable properties for form fields
    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string confirmPassword;

    [ObservableProperty]
    private bool isBusy;

    private readonly IAppUserService _userService;
    public SettingsViewModel(IAppUserService userService)
    {
        // Store service reference for later use
        _userService = userService;
    }
    // Add this command to your SettingsViewModel class
    [RelayCommand]
    private async Task LogoutAsync()
    {
        try
        {
            IsBusy = true;
            await App.NavigateToLoginShell();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to logout: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
