using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Informatics.MauiSurdeg.Interfaces;
using System.Windows.Input;


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
            // Check if service is available
            if (_userService == null)
            {
                ErrorMessage = "Service not available. Please restart the application.";
                Console.WriteLine("ERROR: _userService is null - service was not injected properly");
                return;
            }

            // Clear any previous error messages
            ErrorMessage = null;
            IsBusy = true;

            // Basic validation
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both username and password.";
                return;
            }

            // Log attempt
            Console.WriteLine($"Attempting to find user with username: {Username}");

            // Attempt to retrieve the user from the database
            var user = await _userService.GetUserByIdAsync(Username);

            // Debug output
            Console.WriteLine($"User lookup result: {(user != null ? "Found" : "Not found")}");

            if (user != null)
            {
                ErrorMessage = "Database connection successful! User found.";
                Console.WriteLine($"User details: ID={user.AppUserID}, Username={user.Username}");
            }
            else
            {
                ErrorMessage = "User not found in database.";
                Console.WriteLine("No matching user found in database.");
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Login error: {ex.Message}";
            Console.WriteLine($"Login error: {ex.Message}");

            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner error: {ex.InnerException.Message}");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
