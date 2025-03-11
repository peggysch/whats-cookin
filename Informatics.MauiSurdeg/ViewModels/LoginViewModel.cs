using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Informatics.MauiSurdeg.ViewModels;

/// <summary>
/// ViewModel for the Login Page, handling user input and login logic.
/// </summary>
public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string errorMessage;

    /// <summary>
    /// Command triggered when the login button is pressed.
    /// </summary>
    public ICommand LoginCommand { get; }
    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(Login);
    }

    /// <summary>
    /// Simulates user authentication.
    /// </summary>
    private void Login()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter both username and password.";
            return;
        }

        // TODO: Replace with real authentication logic.
        if (Username == "admin" && Password == "password")
        {
            ErrorMessage = "Login successful!";
            // Navigate to another page or perform login action.
        }
        else
        {
            ErrorMessage = "Invalid username or password.";
        }
    }
}
