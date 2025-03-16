using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Informatics.MauiSurdeg.Interfaces;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informatics.MauiSurdeg.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        // Observable properties for form fields
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string displayUsername;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private bool canExecuteSavePassword;

        private readonly IAppUserService _userService;
        private readonly ProfileViewModel _profileViewModel;


        public ICommand SaveUsernameCommand { get; }
        public ICommand SavePasswordCommand { get; }

        public SettingsViewModel(IAppUserService userService, ProfileViewModel profileViewModel)
        {
            _userService = userService;
            _profileViewModel = profileViewModel;
            SaveUsernameCommand = new AsyncRelayCommand(SaveUsernameAsync);
            SavePasswordCommand = new AsyncRelayCommand(SavePasswordAsync);
        }

        public async Task LoadUserProfileAsync()
        {
            try
            {
                IsBusy = true;
                await _profileViewModel.LoadUserProfileAsync(); // Load user profile

                int userId = App.UserId; // Retrieve user ID globally from App
                if (userId > 0)
                {
                    var user = await _userService.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        DisplayUsername = user.Username; // Set the Username property
                    }
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load user profile: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveUsernameAsync()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                await Shell.Current.DisplayAlert("Error", "Username cannot be empty.", "OK");
                return;
            }

            try
            {
                IsBusy = true;

                int userId = App.UserId; // Get authenticated user ID
                if (userId == 0)
                {
                    await Shell.Current.DisplayAlert("Error", "User not found.", "OK");
                    return;
                }

                // Check if the new username already exists
                bool usernameExists = await _userService.UsernameExistsAsync(Username);
                if (usernameExists)
                {
                    await Shell.Current.DisplayAlert("Error", "Username already taken.", "OK");
                    return;
                }

                // Update username in database
                bool success = await _userService.UpdateUsernameAsync(userId, Username);
                if (success)
                {
                    await Shell.Current.DisplayAlert("Success", "Username updated successfully!", "OK");

                    DisplayUsername = Username;
                    ClearFields();

                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to update username.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Unexpected error: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        private async Task SavePasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await Shell.Current.DisplayAlert("Error", "Password fields cannot be empty.", "OK");
                return;
            }

            if (Password.Length < 5)
            {
                await Shell.Current.DisplayAlert("Error", "Password must be at least 5 characters long.", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await Shell.Current.DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            try
            {
                IsBusy = true;
                int userId = App.UserId;

                if (userId == 0)
                {
                    await Shell.Current.DisplayAlert("Error", "User not found.", "OK");
                    return;
                }
                //Calls the UpdatePasswordAsync method in the AppUserService
                bool success = await _userService.UpdatePasswordAsync(userId, Password);

                if (success)
                {
                    await Shell.Current.DisplayAlert("Success", "Password updated successfully!", "OK");

                    ClearFields();

                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to update password.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Unexpected error: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            try
            {
                IsBusy = true;

                // Clear stored username and password before navigating to login page
                var loginViewModel = App.Current.Handler.MauiContext.Services.GetService<LoginViewModel>();
                if (loginViewModel != null)
                {
                    loginViewModel.Username = string.Empty;
                    loginViewModel.Password = string.Empty;
                }
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
        private void ClearFields()
        {
            Username = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }
    }
}