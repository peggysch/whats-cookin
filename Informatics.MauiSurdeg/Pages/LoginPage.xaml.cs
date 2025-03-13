using Informatics.MauiSurdeg.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace Informatics.MauiSurdeg.Pages;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        bool isAuthenticated = await _viewModel.LoginAsync(); // Uses your existing login logic

        if (isAuthenticated)
        {
            // Enable the flyout menu and show navigation options
            AppShell.Current.IsUserLoggedIn = true;

            // Navigate to the feed page
            await Shell.Current.GoToAsync("//feed");
        }
        else
        {
            await DisplayAlert("Login Failed", "Invalid username or password. Please try again.", "OK");
        }
    }
}
