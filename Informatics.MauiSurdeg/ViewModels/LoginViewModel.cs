using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Pages;
using System.Threading.Tasks;
using System.Windows.Input;

//Profile
using Microsoft.Maui.Storage;
using Informatics.MauiSurdeg.Models;
using System.ComponentModel.DataAnnotations;


namespace Informatics.MauiSurdeg.ViewModels;

/// <summary>
/// ViewModel for the Login Page, handling user input and login logic.
/// </summary>
public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty]
    private string newUsername;

    [ObservableProperty]
    private string newPassword;

    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string errorMessage;

    private readonly IAppUserService _userService;

    public ICommand LoginCommand { get; } //Command for logging in
    public ICommand CreateUserCommand { get; } // Command for creating a new user

    public LoginViewModel(IAppUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        LoginCommand = new AsyncRelayCommand(LoginAsync);
        CreateUserCommand = new AsyncRelayCommand(CreateUserAsync);
    }

    /// <summary>
    /// Simulates user login logic.
    /// </summary>
    private async Task LoginAsync()
    {
        try
        {
            // Clear previous error messages
            ErrorMessage = string.Empty;
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

            // Retrieve the authenticated user ID which is nullable
            int? userIdNullable = await _userService.GetAuthenticatedUserIdAsync(Username);
            if (userIdNullable == null)
            {
                ErrorMessage = "Failed to retrieve user ID.";
                return;
            }

            int loggedinUserId = userIdNullable.Value; // converting from nullable int to int

            // Clear stored username and password before navigating to login page
            Username = string.Empty;
            Password = string.Empty;

            // Byt till MainShell
            await App.NavigateToMainShell(loggedinUserId);

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
    /// <summary>
    /// Handles new user registration.
    /// </summary>
    private async Task CreateUserAsync()
    {
        try
        {
            // Clear any previous error messages
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(NewUsername) || string.IsNullOrWhiteSpace(NewPassword))
            {
                ErrorMessage = "Username and password cannot be empty.";
                return;
            }

            if (NewUsername.Length < 5) //Denna går såklart att ta bort men tycker det är bra att ha en minsta längd 
            {
                ErrorMessage = "Username must be at least 5 characters long.";
                return;
            }

            // Check if username already exists
            bool userExists = await _userService.UsernameExistsAsync(NewUsername);
            if (userExists)
            {
                ErrorMessage = "Username already exists.";
                return;
            }

            // Register the new user
            var newUser = await _userService.AddNewUserAsync(NewUsername, NewPassword);
            if (newUser != null)
            {
                await Shell.Current.DisplayAlert("Success", "Account created! Please log in.", "OK");

                //Clear input fields
                NewUsername = string.Empty;
                NewPassword = string.Empty; //KOLLA PÅ DETTA
            }
            else
            {
                ErrorMessage = "Failed to create user.";
            }
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Username already exists"))
        {
            ErrorMessage = "Username already exists.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An unexpected error occurred, please try again: {ex.Message}";
        }
    }
}