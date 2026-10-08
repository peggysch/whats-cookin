using Informatics.MauiSurdeg.ViewModels;
using Informatics.MauiSurdeg.Pages;

namespace Informatics.MauiSurdeg;

public partial class App : Application
{
    private static App _instance;
    public static int UserId { get; private set; }

    public App()
    {
        InitializeComponent();
        UserAppTheme = AppTheme.Light; // The app is designed for light mode
        MainPage = new LoginShell();
        _instance = this; // Spara referensen till instansen av appen
    }

    public static async Task NavigateToMainShell(int userId)
    {
        if (_instance?.MainPage != null)
        {
            await _instance.MainPage.Dispatcher.DispatchAsync(() =>
            {
                UserId = userId;
                _instance.MainPage = new MainShell();
            });
        }
    }

    public static async Task NavigateToLoginShell()
    {
        if (_instance?.MainPage != null)
        {
            await _instance.MainPage.Dispatcher.DispatchAsync(() =>
            {
                _instance.MainPage = new LoginShell();
            });
        }
    }
}
