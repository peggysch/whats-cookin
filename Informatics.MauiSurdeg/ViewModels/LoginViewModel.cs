using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Pages;
using System.Windows.Input;

//Profile
using Microsoft.Maui.Storage;
using Informatics.MauiSurdeg.Models;


namespace Informatics.MauiSurdeg.ViewModels;

/// <summary>
/// ViewModel for the Login Page, handling user input and login logic.
/// </summary>
public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty]
    private string newUsername;

    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string errorMessage;

    private readonly IAppUserService _userService;

    public ICommand LoginCommand { get; }

    public LoginViewModel(IAppUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        LoginCommand = new AsyncRelayCommand(LoginAsync);
    }

    /// <summary>
    /// Simulates user authentication.
    /// </summary>
    private async Task LoginAsync()
    {
        try
        {
            // Clear previous error messages
            ErrorMessage = null;
            IsBusy = true;

            // Basic validation
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both username and password.";
                return;
            }

            // Check if the user exists
            bool userExists = await _userService.UsernameExistsAsync(Username);
            if (!userExists)
            {
                ErrorMessage = "User does not exist.";
                return;
            }

            // Authenticate user credentials
            bool isAuthenticated = await _userService.AuthenticateUserAsync(Username, Password);
            if (!isAuthenticated)
            {
                ErrorMessage = "Invalid username or password.";
                return;
            }

            // Retrieve the authenticated user ID
            var userId = await _userService.GetAuthenticatedUserIdAsync(Username);
            if (userId == null)
            {
                ErrorMessage = "Failed to retrieve user ID.";
                return;
            }

            // Byt till MainShell
            await App.NavigateToMainShell();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }



    // TODO: men inte nu, Registera konto :)


}
